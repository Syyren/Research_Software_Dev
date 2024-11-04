using Research_Software_Dev.Models.Roles;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherRole
    {
        public ResearcherRole()
        {
            ResearcherRoleId = int.Parse($"{ResearcherId}{RoleId}");
        }
        public int ResearcherRoleId { get; }

        //Researcher FK
        public int ResearcherId { get; set; }
        public Researcher Researcher { get; set; }

        //Role FK
        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}
