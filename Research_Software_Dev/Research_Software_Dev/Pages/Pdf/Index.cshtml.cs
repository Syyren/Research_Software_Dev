using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Services;
using System.IO;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Pdf
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly PdfInterpreter _pdfInterpreter;

        [BindProperty]
        public IFormFile PdfFile { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
            _pdfInterpreter = new PdfInterpreter();
        }

        public void OnGet()
        {
            // Loads any necessary data or initialize the page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (PdfFile == null || PdfFile.Length == 0)
            {
                ViewData["Message"] = "No file selected for upload.";
                return Page();
            }

            try
            {
                // Defines the upload folder path
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Ensures the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Defines the full file path
                var filePath = Path.Combine(uploadDir, PdfFile.FileName);

                // Saves the file to the uploads folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PdfFile.CopyToAsync(stream);
                }

                // Parses the PDF
                var (form, questions, answers) = _pdfInterpreter.ParsePdf(filePath);

                form.FormId = Guid.NewGuid().ToString();

                // Saves the parsed data to the database
                _context.Forms.Add(form); // Saves the Form
                await _context.SaveChangesAsync(); // Save to generate FormId

                foreach (var question in questions)
                {
                    question.FormId = form.FormId; // Associates the question with the Form
                    _context.FormQuestions.Add(question);
                    question.QuestionNumber = question.QuestionNumber + 1;
                }

                await _context.SaveChangesAsync(); // Saves all questions

                // Redirects to the Results page with data
                return RedirectToPage("/Forms/Index");
            }
            catch (Exception ex)
            {
                ViewData["Message"] = $"Internal server error: {ex.Message}";
                return Page();
            }
        }
    }
}