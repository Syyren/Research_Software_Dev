using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Research_Software_Dev.Models.Forms
{
    public enum QuestionType
    {
        SingleChoice,
        LikertScale,
        FreeText
    }

    public class FormQuestion
    {
        [Key]
        public string FormQuestionId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int QuestionNumber { get; set; }

        [Required]
        [Display(Name = "Question Description")]
        public string QuestionDescription { get; set; }

        [Required]
        [Display(Name = "Question Type")]
        public QuestionType Type { get; set; }

        // Metadata for choices, scales, etc.
        public string? OptionsJson { get; set; }

        // Foreign Key
        [Required]
        public string FormId { get; set; }
        [ForeignKey("FormId")]
        public Form? Form { get; set; }

        public List<string> GetOptions()
        {
            if (string.IsNullOrWhiteSpace(OptionsJson))
            {
                return new List<string>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<string>>(OptionsJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>(); // Return empty list if deserialization fails
            }
        }

            // Constructors
        public FormQuestion() { }

        public FormQuestion(string questionId, int questionNumber, string questionDescription, QuestionType type, string formId, string? optionsJson = null)
        {
            FormQuestionId = questionId;
            QuestionNumber = questionNumber;
            QuestionDescription = questionDescription;
            Type = type;
            FormId = formId;
            OptionsJson = optionsJson;
        }
    }
}