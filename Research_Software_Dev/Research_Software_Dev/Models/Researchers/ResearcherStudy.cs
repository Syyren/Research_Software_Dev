using Research_Software_Dev.Models.Studies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherStudy
    {
        //Composite key auto generated in context

        //Researcher FK
        [Required]
        public string ResearcherId { get; set; }
        [ForeignKey("Id")]
        public Researcher Researcher { get; set; }

        //Study FK
        [Required]
        public string StudyId { get; set; }
        [Required]
        public string StudyName { get; set; }
        [ForeignKey("StudyId")]
        public Study Study { get; set; }

        //Constructors
        public ResearcherStudy() { }
        public ResearcherStudy(string researcherId, string studyId, string studyName)
        {
            ResearcherId = researcherId;
            StudyId = studyId;
            StudyName = studyName;
        }
    }
}
