using Microsoft.AspNetCore.Mvc;
using Research_Software_Dev.Services;

namespace Research_Software_Dev.Controllers
{
    public class PdfController : Controller
    {
        private readonly PdfInterpreter _pdfInterpreter;

        public PdfController()
        {
            // Initialize PdfInterpreter service
            _pdfInterpreter = new PdfInterpreter();
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Render the upload form
            return View("Index");
        }

        [HttpPost]
        [Route("Pdf/UploadPdf")]
        public IActionResult UploadPdf([FromForm] IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return BadRequest("No file selected for upload.");
            }

            try
            {
                // Define the upload folder path
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Ensure the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Define the full file path
                var filePath = Path.Combine(uploadDir, pdfFile.FileName);

                // Save the file to the uploads folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    pdfFile.CopyTo(stream);
                }

                var pdfInterpreter = new PdfInterpreter();
                var (form, questions, answers) = pdfInterpreter.ParsePdf(filePath);

                ViewBag.Form = form;
                ViewBag.Questions = questions;
                ViewBag.Answers = answers;

                return View("Results");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}