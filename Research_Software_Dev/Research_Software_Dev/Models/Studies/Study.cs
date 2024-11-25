using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Studies
{
    public class Study
    {
        [Key]
        public string StudyId { get; set; }

        [Required]
        [Display(Name = "Study Name")]
        public string? StudyName { get; set; }

        [Required]
        [Display(Name = "Study Description")]
        public string? StudyDescription { get; set; }
        
        //Constructors
        public Study() { }

        public Study(string id, string name, string desc)
        {
            StudyId = id;
            StudyName = name;
            StudyDescription = desc;
        }
    }
}
