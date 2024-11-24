using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class FormQuestion
    {
        [Key]
        public int QuestionId { get; set; }
        [Required]
        [StringLength(10)]
        public string QuestionNumber { get; set; }
        [Required]
        public string QuestionDescription { get; set; }

        //Form FK
        [Required]
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public Form Form { get; set; }

        //Constructors
        public FormQuestion() { }
        public FormQuestion(int questionId, string questionNumber, string questionDescription, int formId)
        {
            QuestionId = questionId;
            QuestionNumber = questionNumber;
            QuestionDescription = questionDescription;
            FormId = formId;
        }
    }
}
