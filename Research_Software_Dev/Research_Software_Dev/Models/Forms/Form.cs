using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class Form
    {
        public string FormId { get; set; }
        [Required]
        public string FormName { get; set; }
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
