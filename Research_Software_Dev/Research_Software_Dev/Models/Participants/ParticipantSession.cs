using Research_Software_Dev.Models.Sessions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Participants
{
    public class ParticipantSession
    {
        //Composite key auto generated in context

        //Participant FK
        [Required]
        public int ParticipantId { get; set; }

        [ForeignKey("ParticipantId")]
        public Participant Participant { get; set; }

        //Session FK
        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public Session Session { get; set; }

        //Constructors
        public ParticipantSession() { }
        public ParticipantSession(int participantId, int sessionId)
        {
            ParticipantId = participantId;
            SessionId = sessionId;
        }
    }
}
