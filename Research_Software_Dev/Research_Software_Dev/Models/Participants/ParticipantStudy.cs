using Research_Software_Dev.Models.Studies;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Participants
{
    public class ParticipantStudy
    {
        //Composite key auto generated in context

        [Required]
        public string ParticipantId { get; set; }

        [ForeignKey("ParticipantId")]
        public Participant Participant { get; set; }


        [Required]
        public string StudyId { get; set; }

        [ForeignKey("StudyId")]
        public Study Study { get; set; }

        //Constructors
        public ParticipantStudy() { }
        public ParticipantStudy(string participantId, string studyId)
        {
            ParticipantId = participantId;
            StudyId = studyId;
        }
    }
}
