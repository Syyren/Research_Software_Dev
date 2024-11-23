using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Forms
{
    public class Form
    {
        [Key]
        public int FormId { get; set; }
        [Required]
        public string FormName { get; set; }
    }
}
