using Research_Software_Dev.Models.Studies;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Participants
{
    public class ParticipantStudy(int participantId, int studyId)
    {
        //Composite key auto generated in context

        [Required]
        [ForeignKey("Participant")]
        public int ParticipantId { get; set; } = participantId;

        [Required]
        public Participant Participant { get; set; }


        [Required]
        [ForeignKey("Study")]
        public int StudyId { get; set; } = studyId;

        [Required]
        public Study Study { get; set; }
    }
}
