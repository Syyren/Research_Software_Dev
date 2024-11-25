using Aspose.Pdf;
using Aspose.Pdf.Text;
using Research_Software_Dev.Models.Forms;

namespace Research_Software_Dev.Services
{
    public class PdfInterpreter
    {
        public (Form, List<FormQuestion>, List<FormAnswer>) ParsePdf(string pdfPath)
        {
            var form = new Form();
            var questions = new List<FormQuestion>();
            var answers = new List<FormAnswer>();
            bool isFirstLine = true;

            // Load the PDF document
            var document = new Aspose.Pdf.Document(pdfPath);

            // Loop through each page in the document
            foreach (Page page in document.Pages)
            {
                // Extract text from the page
                var textAbsorber = new TextAbsorber();
                page.Accept(textAbsorber);
                var pageText = textAbsorber.Text;

                // Process the extracted text
                ProcessPageContent(pageText, form, questions, ref isFirstLine);
            }

            return (form, questions, answers);
        }

        private void ProcessPageContent(string content, Form form, List<FormQuestion> questions, ref bool isFirstLine)
        {
            var lines = content.Split('\n').Select(line => line.Trim()).Where(line => !string.IsNullOrEmpty(line)).ToList();
            foreach (var line in lines)
            {
                if (isFirstLine)
                {
                    form.FormName = line;
                    isFirstLine = false;
                    continue;
                }

                if (IsQuestion(line))
                {
                    string description = ExtractQuestionDescription(line);

                    // Ensure the description is valid
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        throw new InvalidOperationException("Invalid question description detected in the PDF.");
                    }

                    var question = new FormQuestion
                    {
                        FormQuestionId = Guid.NewGuid().ToString(),
                        QuestionNumber = ExtractQuestionNumber(line),
                        QuestionDescription = description,
                        FormId = form.FormId,
                        Form = form
                    };

                    questions.Add(question);
                }
            }
        }

        private bool IsQuestion(string line)
        {
            // Check if the line starts with a number and has text
            var parts = line.Split(' ', 2); // Split into at most 2 parts
            return parts.Length > 1 && int.TryParse(parts[0], out _);
        }

        private int ExtractQuestionNumber(string line)
        {
            // Extract the number at the start of the line
            var parts = line.Split(' ', 2);
            return int.TryParse(parts[0], out int number) ? number : 0;
        }

        private string ExtractQuestionDescription(string line)
        {
            // Remove the leading number and return the question text
            var firstSpaceIndex = line.IndexOf(' ');
            return firstSpaceIndex >= 0 ? line.Substring(firstSpaceIndex + 1).Trim() : line.Trim();
        }
    }
}