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
            // Load any necessary data or initialize the page
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
                // Define the upload folder path
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Ensure the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Define the full file path
                var filePath = Path.Combine(uploadDir, PdfFile.FileName);

                // Save the file to the uploads folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PdfFile.CopyToAsync(stream);
                }

                // Parse the PDF
                var (form, questions, answers) = _pdfInterpreter.ParsePdf(filePath);

                // Save the parsed data to the database
                _context.Forms.Add(form); // Save the Form
                await _context.SaveChangesAsync(); // Save to generate FormId

                foreach (var question in questions)
                {
                    question.FormId = form.FormId; // Associate the question with the Form
                    _context.FormQuestions.Add(question);
                    question.QuestionNumber = question.QuestionNumber + 1;
                }

                await _context.SaveChangesAsync(); // Save all questions

                // Optional: Add answers (depends on your app's workflow)
                foreach (var answer in answers)
                {
                    _context.FormAnswers.Add(answer); // Save answers (if applicable)
                }

                await _context.SaveChangesAsync();

                // Redirect to the Results page with data
                return RedirectToPage("/Pdf/Results", new { formId = form.FormId });
            }
            catch (Exception ex)
            {
                ViewData["Message"] = $"Internal server error: {ex.Message}";
                return Page();
            }
        }
    }
}