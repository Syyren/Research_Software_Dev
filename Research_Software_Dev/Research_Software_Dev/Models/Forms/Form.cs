using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Forms
{
    public class Form(int id, string formName)
    {
        [Key]
        public int FormId { get; set; } = id;
        [Required]
        public string FormName { get; set; } = formName;
    }
}
