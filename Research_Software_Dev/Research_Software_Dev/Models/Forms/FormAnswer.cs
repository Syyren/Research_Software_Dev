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
        public string TextAnswer { get; set; } // Stores the textual response or selected option text

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime TimeStamp { get; set; }

        // Optional numeric value for the selected choice
        public double? ChoiceValue { get; set; } // Nullable to support free text or unanswered options

        // ParticipantSession composite FK
        [Required]
        public string ParticipantId { get; set; }
        [Required]
        public string SessionId { get; set; }

        [ForeignKey("ParticipantId, SessionId")]
        public ParticipantSession? ParticipantSession { get; set; }

        // FormQuestion FK
        [Required]
        public string FormQuestionId { get; set; }
        [ForeignKey("FormQuestionId")]
        public FormQuestion? FormQuestion { get; set; }

        // Constructors
        public FormAnswer() { }

        public FormAnswer(string answerId, string textAnswer, DateTime timeStamp, string participantId, string sessionId, string formQuestionId, double? choiceValue = null)
        {
            AnswerId = answerId;
            TextAnswer = textAnswer;
            TimeStamp = timeStamp;
            ParticipantId = participantId;
            SessionId = sessionId;
            FormQuestionId = formQuestionId;
            ChoiceValue = choiceValue;
        }
    }
}
