using Research_Software_Dev.Models.Participants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class FormAnswer(int id, string answer, DateTime timeStamp, int participantSessionId, int questionId)
    {
        [Key]
        public int AnswerId { get; set; } = id;
        [Required]
        public string Answer { get; set; } = answer;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; } = timeStamp;

        //ParticipantSession FK
        [Required]
        [ForeignKey("ParticipantSession")]
        public int ParticipantSessionId { get; set; } = participantSessionId;
        [Required]
        public ParticipantSession ParticipantSession { get; set; }

        //Question FK
        [Required]
        [ForeignKey("FormQuestion")]
        public int QuestionId { get; set; } = questionId;
        [Required]
        public FormQuestion Question { get; set; }
    }
}
