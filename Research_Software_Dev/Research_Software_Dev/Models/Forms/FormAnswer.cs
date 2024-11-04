using Research_Software_Dev.Models.Participants;

namespace Research_Software_Dev.Models.Forms
{
    public class FormAnswer
    {
        public int AnswerId { get; set; }
        public string Answer { get; set; }
        public DateTime TimeStamp { get; set; }

        //ParticipantSession FK
        public int ParticipantSessionId { get; set; }
        public ParticipantSession ParticipantSession { get; set; }

        //Question FK
        public string QuestionId { get; set; }
        public string Question { get; set; }
    }
}
