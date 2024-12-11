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

        [Display(Name = "Category")]
        public string? Category { get; set; }

        public List<FormQuestionOption> Options { get; set; } = new();

        // Foreign Key
        [Required]
        public string FormId { get; set; }
        [ForeignKey("FormId")]
        public Form? Form { get; set; }

        // Constructors
        public FormQuestion() { }

        public FormQuestion(string questionId, int questionNumber, string questionDescription, QuestionType type, string formId, List<FormQuestionOption> options, string? category = null)
        {
            FormQuestionId = questionId;
            QuestionNumber = questionNumber;
            QuestionDescription = questionDescription;
            Type = type;
            FormId = formId;
            Category = category;
            Options = options;
        }
    }
}
