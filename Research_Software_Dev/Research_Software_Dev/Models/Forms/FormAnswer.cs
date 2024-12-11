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
<<<<<<< Updated upstream
        public string? TextAnswer { get; set; }

        public string? SelectedOption { get; set; } // Field for storing the selected option ID
=======
        public string TextAnswer { get; set; } // Stores the textual response or selected option text
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
        public FormAnswer(string answerId, string? textAnswer, string? selectedOption, DateTime timeStamp,
                          string participantId, string sessionId, string formQuestionId)
=======
        public FormAnswer(string answerId, string textAnswer, DateTime timeStamp, string participantId, string sessionId, string formQuestionId, double? choiceValue = null)
>>>>>>> Stashed changes
        {
            AnswerId = answerId;
            TextAnswer = textAnswer;
            SelectedOption = selectedOption;
            TimeStamp = timeStamp;
            ParticipantId = participantId;
            SessionId = sessionId;
            FormQuestionId = formQuestionId;
            ChoiceValue = choiceValue;
        }
    }
}
