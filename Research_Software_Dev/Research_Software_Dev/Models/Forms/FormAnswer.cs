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
        [ForeignKey("ParticipantSession")]
        public int ParticipantSessionId { get; set; }
        [Required]
        public ParticipantSession ParticipantSession { get; set; }

        //Question FK
        [Required]
        [ForeignKey("FormQuestion")]
        public string QuestionId { get; set; }
        [Required]
        public FormQuestion Question { get; set; }
    }
}
