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
        public string? TextAnswer { get; set; }

        public string? SelectedOption { get; set; } // Field for storing the selected option ID

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }

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

        public FormAnswer() { }

        public FormAnswer(string answerId, string? textAnswer, string? selectedOption, DateTime timeStamp,
                          string participantId, string sessionId, string formQuestionId)
        {
            AnswerId = answerId;
            TextAnswer = textAnswer;
            SelectedOption = selectedOption;
            TimeStamp = timeStamp;
            ParticipantId = participantId;
            SessionId = sessionId;
            FormQuestionId = formQuestionId;
        }
    }
}
