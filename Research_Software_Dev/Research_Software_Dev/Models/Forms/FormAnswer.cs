using Research_Software_Dev.Models.Participants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Forms
{
    public class FormAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
        public string Answer { get; set; }
        public DateTime TimeStamp { get; set; }

        //ParticipantSession FK
        public int ParticipantSessionId { get; set; }
        //public ParticipantSession ParticipantSession { get; set; }

        //Question FK
        public string QuestionId { get; set; }
        public FormQuestion Question { get; set; }
        public int? FormId { get; internal set; }
    }
}
