using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherSession
    {
        public ResearcherSession()
        {
            ResearcherSessionId = int.Parse($"{ResearcherId}{SessionId}");
        }
        public int ResearcherSessionId { get; }

        //Researcher FK
        public int ResearcherId { get; set; }
        public Researcher Researcher { get; set; }

        //Session FK
        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
