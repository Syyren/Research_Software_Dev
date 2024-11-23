using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class Researcher(int id, string first_name, string last_name, string address, string email, string phone_number, string password)
    {
        [Key]
        public int ResearcherId { get; set; } = id;

        [Required]
        [StringLength(50)]
        public string ResearcherFirstName { get; set; } = first_name;

        [Required]
        [StringLength(50)]
        public string ResearcherLastName { get; set; } = last_name;

        [Required]
        [StringLength(255)]
        public string ResearcherAddress { get; set; } = address;

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string ResearcherEmail { get; set; } = email;

        [Required]
        [Phone]
        [StringLength(50)]
        public string ResearcherPhoneNumber { get; set; } = phone_number;

        [Required]
        [StringLength(50)]
        public string ResearcherPassword { get; set; } = password;
    }
}
