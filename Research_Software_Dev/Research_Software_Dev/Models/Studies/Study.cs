using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Studies
{
    public class Study
    {
        [Key]
        public int StudyId { get; set; }
        [Required]
        [StringLength(50)]
        public string StudyName { get; set; }
        [Required]
        public string StudyDescription { get; set; }
    }

}
