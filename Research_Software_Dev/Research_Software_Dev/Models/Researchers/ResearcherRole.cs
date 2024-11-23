using Research_Software_Dev.Models.Roles;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherRole(int researcherId, int roleId)
    {
        //Composite key auto generated in context

        [Required]
        [ForeignKey("Researcher")]
        public int ResearcherId { get; set; } = researcherId;

        [Required]
        public Researcher Researcher { get; set; }


        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; } = roleId;

        [Required]
        public Role Role { get; set; }
    }
}
