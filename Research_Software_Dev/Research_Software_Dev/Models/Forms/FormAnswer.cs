using Research_Software_Dev.Models.Participants;
<<<<<<< HEAD
=======
using Research_Software_Dev.Models.Sessions;
>>>>>>> dev
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class FormAnswer
    {
        [Key]
<<<<<<< HEAD
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
=======
        public string AnswerId { get; set; }
        [Required]
>>>>>>> dev
        public string Answer { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

<<<<<<< HEAD
        //ParticipantSession FK
        public int ParticipantSessionId { get; set; }
        //public ParticipantSession ParticipantSession { get; set; }
=======
        //ParticipantSession composite FK
        [Required]
        public string ParticipantId { get; set; }
        [Required]
        public string SessionId { get; set; }

        [ForeignKey("ParticipantId, SessionId")]
        public ParticipantSession ParticipantSession { get; set; }
>>>>>>> dev

        //FormQuestion FK
        [Required]
        public string QuestionId { get; set; }
<<<<<<< HEAD
        public FormQuestion Question { get; set; }
        public int? FormId { get; internal set; }
=======
        [ForeignKey("FormQuestionId")]
        public FormQuestion FormQuestion { get; set; }

        //Constructors
        public FormAnswer() { }
        public FormAnswer(string answerId, string answer, DateTime timeStamp, string participantId, 
            string sessionId, string questionId)
        {
            AnswerId = answerId;
            Answer = answer;
            TimeStamp = timeStamp;
            ParticipantId = participantId;
            SessionId = sessionId;
            QuestionId = questionId;
        }
>>>>>>> dev
    }
}
