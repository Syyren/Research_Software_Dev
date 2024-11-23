using Research_Software_Dev.Models.Sessions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Participants
{
    public class ParticipantSession
    {
        //Composite key auto generated in context

        [Required]
        [ForeignKey("Participant")]
        public int ParticipantId { get; set; }

        [Required]
        public Participant Participant { get; set; }


        [Required]
        [ForeignKey("Session")]
        public int SessionId { get; set; }

        [Required]
        public Session Session { get; set; }
    }
}
