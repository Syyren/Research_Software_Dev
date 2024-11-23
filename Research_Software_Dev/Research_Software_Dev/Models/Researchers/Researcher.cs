using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class Researcher
    {
        [Key]
        public int ResearcherId { get; set; }

        [Required]
        [StringLength(50)]
        public string ResearcherFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string ResearcherLastName { get; set; }

        [Required]
        [StringLength(255)]
        public string ResearcherAddress { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string ResearcherEmail { get; set; }

        [Required]
        [Phone]
        [StringLength(50)]
        public string ResearcherPhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string ResearcherPassword { get; set; }
    }
}
