using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Models.Participants
{
    public class ParticipantStudy
    {
        public ParticipantStudy(Participant participant, Study study)
        {
            ParticipantId = participant.ParticipantId;
            ParticipantStudyId = int.Parse($"{ParticipantId}{StudyId}");
        }
        public int ParticipantStudyId { get; }

        //Participant FK
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        //Study FK
        public int StudyId { get; set; }
        public Study Study { get; set; }
    }
}
