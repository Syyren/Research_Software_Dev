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
        public int QuestionNumber { get; set; }
        [Required]
        public string QuestionDescription { get; set; }

        //Form FK
        [Required]
        [ForeignKey("Form")]
        public int FormId { get; set; }
        [Required]
        public Form Form { get; set; }
    }
}
