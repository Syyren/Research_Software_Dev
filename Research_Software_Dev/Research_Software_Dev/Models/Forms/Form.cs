using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class Form
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormId { get; set; }
        public string FormName { get; set; }
        public ICollection<FormQuestion> Questions { get; set; } = new List<FormQuestion>();
    }
}
