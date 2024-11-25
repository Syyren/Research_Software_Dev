using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class Researcher : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Researcher First Name")]
        public string ResearcherFirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Researcher Last Name")]
        public string ResearcherLastName { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Researcher Address")]
        public string ResearcherAddress { get; set; }

        //Constructors
        public Researcher() { }
        public Researcher(string researcherFirstName, string researcherLastName, string researcherAddress,
                          string email, string phoneNumber, string password)
            : base()
        {
            ResearcherFirstName = researcherFirstName;
            ResearcherLastName = researcherLastName;
            ResearcherAddress = researcherAddress;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
