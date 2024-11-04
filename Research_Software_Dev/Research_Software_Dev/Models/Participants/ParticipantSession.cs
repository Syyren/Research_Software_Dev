using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Models.Participants
{
    public class ParticipantSession
    {
        public ParticipantSession()
        {
            ParticipantSessionId = int.Parse($"{ParticipantId}{SessionId}");
        }
        public int ParticipantSessionId { get; }

        //Participant FK
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        //Session FK
        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
