using Research_Software_Dev.Models.Participants;
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

        //ParticipantSession FK
        [Required]
        public int ParticipantSessionId { get; set; }

        [ForeignKey("ParticipantSessionId")]
        public ParticipantSession ParticipantSession { get; set; }

        //FormQuestion FK
        [Required]
        public int QuestionId { get; set; }
        [ForeignKey("FormQuestionId")]
        public FormQuestion FormQuestion { get; set; }

        //Constructors
        public FormAnswer() { }
        public FormAnswer(int answerId, string answer, DateTime timeStamp, int participantSessionId, ParticipantSession participantSession, int questionId, FormQuestion formQuestion)
        {
            AnswerId = answerId;
            Answer = answer;
            TimeStamp = timeStamp;
            ParticipantSessionId = participantSessionId;
            ParticipantSession = participantSession;
            QuestionId = questionId;
            FormQuestion = formQuestion;
        }
    }
}
