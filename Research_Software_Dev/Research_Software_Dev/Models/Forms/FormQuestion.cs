using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Research_Software_Dev.Models.Forms
{
    public class FormQuestion
    {
        [Key]
        public string QuestionId { get; set; } = Guid.NewGuid().ToString();
        public int QuestionNumber { get; set; }
        public string QuestionDescription { get; set; }

        //Form FK
        public int FormId { get; set; }
        public Form? Form { get; set; }
    }
}
