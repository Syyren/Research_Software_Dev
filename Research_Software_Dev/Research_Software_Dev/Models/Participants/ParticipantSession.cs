using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Models.Participants
{
    public class ParticipantSession
    {
        public ParticipantSession(Participant participant, Session session)
        {
            ParticipantId = participant.ParticipantId;
            SessionId = session.SessionId;
            ParticipantSessionId = int.Parse($"{this.ParticipantId}{this.SessionId}");
        }
        public int ParticipantSessionId { get; }

        //Participant FK
        public int ParticipantId { get; set; }

        //Session FK
        public int SessionId { get; set; }
    }
}
