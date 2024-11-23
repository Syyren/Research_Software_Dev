using Research_Software_Dev.Models.Sessions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherSession
    {
        //Composite key auto generated in context

        [Required]
        [ForeignKey("Researcher")]
        public int ResearcherId { get; set; }

        [Required]
        public Researcher Researcher { get; set; }


        [Required]
        [ForeignKey("Session")]
        public int SessionId { get; set; }

        [Required]
        public Session Session { get; set; }
    }
}
