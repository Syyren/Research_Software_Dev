using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Studies
{
    public class Study
    {
        public int StudyId { get; set; }

        [Required]
        public string? StudyName { get; set; }

        [Required]
        public string? StudyDescription { get; set; }
        
        //Constructors
        public Study() { }

        public Study(int id, string name, string desc)
        {
            StudyId = id;
            StudyName = name;
            StudyDescription = desc;
        }
    }
}
