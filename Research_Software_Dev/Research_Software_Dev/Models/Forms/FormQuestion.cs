using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class FormQuestion
    {
        [Key]
        public string FormQuestionId { get; set; }
        [Required]
        [StringLength(10)]
        public string FormQuestionNumber { get; set; }
        [Required]
        public string FormQuestionDescription { get; set; }

        //Form FK
        [Required]
        public string FormId { get; set; }
        [ForeignKey("FormId")]
        public Form Form { get; set; }

        //Constructors
        public FormQuestion() { }
        public FormQuestion(string questionId, string questionNumber, string questionDescription, string formId)
        {
            FormQuestionId = questionId;
            FormQuestionNumber = questionNumber;
            FormQuestionDescription = questionDescription;
            FormId = formId;
        }
    }
}
