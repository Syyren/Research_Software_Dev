using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class FormAnswer
    {
        [Key]
        public int AnswerId { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

        //ParticipantSession composite FK
        [Required]
        public int ParticipantId { get; set; }
        [Required]
        public int SessionId { get; set; }

        [ForeignKey("ParticipantId, SessionId")]
        public ParticipantSession ParticipantSession { get; set; }

        //FormQuestion FK
        [Required]
        public int QuestionId { get; set; }
        [ForeignKey("FormQuestionId")]
        public FormQuestion FormQuestion { get; set; }

        //Constructors
        public FormAnswer() { }
        public FormAnswer(int answerId, string answer, DateTime timeStamp, int participantId, int sessionId, int questionId)
        {
            AnswerId = answerId;
            Answer = answer;
            TimeStamp = timeStamp;
            ParticipantId = participantId;
            SessionId = sessionId;
            QuestionId = questionId;
        }
    }
}
