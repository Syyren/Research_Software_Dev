using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> dev

namespace Research_Software_Dev.Models.Forms
{
    public class Form
    {
        [Key]
<<<<<<< HEAD
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormId { get; set; }
        public string FormName { get; set; }
        public ICollection<FormQuestion> Questions { get; set; } = new List<FormQuestion>();
=======
        public string FormId { get; set; }
        [Required]
        public string FormName { get; set; }

        //Constructors
        public Form() { }
        public Form(string formId, string formName)
        {
            FormId = formId;
            FormName = formName;
        }
>>>>>>> dev
    }
}
