using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Research_Software_Dev.Models.Forms
{
    public class FormQuestion
    {
        [Key]
        public string FormQuestionId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public int QuestionNumber { get; set; }
        [Required]
        [Display(Name = "Question Description")]
        public string QuestionDescription { get; set; }

        //Form FK
        [Required]
        public string FormId { get; set; }
        [ForeignKey("FormId")]
        public Form? Form { get; set; }

        //Constructors
        public FormQuestion() { }
        public FormQuestion(string questionId, int questionNumber, string questionDescription, string formId)
        {
            FormQuestionId = questionId;
            QuestionNumber = questionNumber;
            QuestionDescription = questionDescription;
            FormId = formId;
        }
    }
}
