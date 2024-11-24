using Research_Software_Dev.Models.Sessions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherSession
    {
        //Composite key auto generated in context

        [Required]
        public string ResearcherId { get; set; }

        [ForeignKey("Id")]
        public Researcher Researcher { get; set; }


        [Required]
        public string SessionId { get; set; }

        [ForeignKey("SessionId")]
        public Session Session { get; set; }

        //Constructors
        public ResearcherSession() { }
        public ResearcherSession(string researcherId, string sessionId)
        {
            ResearcherId = researcherId;
            SessionId = sessionId;
        }
    }
}
