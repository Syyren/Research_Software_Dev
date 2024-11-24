using Research_Software_Dev.Models.Roles;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherRole
    {
        //Composite key auto generated in context

        [Required]
        public int ResearcherId { get; set; }
        [ForeignKey("ResearcherId")]
        public Researcher Researcher { get; set; }


        [Required]
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        //Constructors
        public ResearcherRole() { }
        public ResearcherRole(int researcherId, int roleId)
        {
            ResearcherId = researcherId;
            RoleId = roleId;
        }
    }
}
