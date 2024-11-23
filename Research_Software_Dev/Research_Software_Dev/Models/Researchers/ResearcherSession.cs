using Research_Software_Dev.Models.Sessions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherSession(int researcherId, int sessionId)
    {
        //Composite key auto generated in context

        [Required]
        [ForeignKey("Researcher")]
        public int ResearcherId { get; set; } = researcherId;

        [Required]
        public Researcher Researcher { get; set; }


        [Required]
        [ForeignKey("Session")]
        public int SessionId { get; set; } = sessionId;

        [Required]
        public Session Session { get; set; }
    }
}
