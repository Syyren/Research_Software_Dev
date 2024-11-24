using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Forms
{
    public class Form
    {
        [Key]
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
    }
}
