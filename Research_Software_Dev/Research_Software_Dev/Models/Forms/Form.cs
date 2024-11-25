using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class Form
    {
        public string FormId { get; set; }

        [Required]
        [Display(Name = "Form Name")]
        public string FormName { get; set; }

        [Display(Name = "Form Questions")]
        public ICollection<FormQuestion> Questions { get; set; } = new List<FormQuestion>();



        //Constructors
        public Form() { }
        public Form(string formId, string formName)
        {
            FormId = formId;
            FormName = formName;
        }
    }
}
