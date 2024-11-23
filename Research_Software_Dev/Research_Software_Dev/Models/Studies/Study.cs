using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Studies
{
    public class Study(int id, string name, string description)
    {
        [Key]
        public int StudyId { get; set; } = id;
        [Required]
        [StringLength(50)]
        public string StudyName { get; set; } = name;
        [Required]
        public string StudyDescription { get; set; } = description;
    }

}
