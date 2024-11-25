using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class FormAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AnswerId { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }
        //ParticipantSession composite FK
        [Required]
        public string ParticipantId { get; set; }
        [Required]
        public string SessionId { get; set; }

        [ForeignKey("ParticipantId, SessionId")]
        public ParticipantSession? ParticipantSession { get; set; }
        //FormQuestion FK
        [Required]
        public string FormQuestionId { get; set; }
        public string? FormId { get; internal set; }
        [ForeignKey("FormQuestionId")]
        public FormQuestion? FormQuestion { get; set; }

        //Constructors
        public FormAnswer() { }
        public FormAnswer(string answerId, string answer, DateTime timeStamp, string participantId, 
            string sessionId, string questionId, string formId)
        {
            AnswerId = answerId;
            Answer = answer;
            TimeStamp = timeStamp;
            ParticipantId = participantId;
            SessionId = sessionId;
            FormQuestionId = questionId;
            FormId = formId;
        }
    }
}
