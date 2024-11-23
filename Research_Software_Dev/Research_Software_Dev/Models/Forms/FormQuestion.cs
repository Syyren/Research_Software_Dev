using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class FormQuestion(int id, string questionNumber, string questionDescription, int formId)
    {
        [Key]
        public int QuestionId { get; set; } = id;
        [Required]
        [StringLength(10)]
        public string QuestionNumber { get; set; } = questionNumber;
        [Required]
        public string QuestionDescription { get; set; } = questionDescription;

        //Form FK
        [Required]
        [ForeignKey("Form")]
        public int FormId { get; set; } = formId;
        [Required]
        public Form Form { get; set; }
    }
}
