using Research_Software_Dev.Models.Sessions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherSession
    {
        //Composite key auto generated in context

        [Required]
        public int ResearcherId { get; set; }

        [ForeignKey("ResearcherId")]
        public Researcher Researcher { get; set; }


        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public Session Session { get; set; }

        //Constructors
        public ResearcherSession() { }
        public ResearcherSession(int researcherId, int sessionId)
        {
            ResearcherId = researcherId;
            SessionId = sessionId;
        }
    }
}
