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
        public int ResearcherId { get; set; }
        [ForeignKey("ResearcherId")]
        public Researcher Researcher { get; set; }

        //Study FK
        [Required]
        public int StudyId { get; set; }
        [Required]
        public string StudyName { get; set; }
        [ForeignKey("StudyId")]
        public Study Study { get; set; }

        //Constructors
        public ResearcherStudy() { }
        public ResearcherStudy(int researcherId, Researcher researcher, int studyId, string studyName, Study study)
        {
            ResearcherId = researcherId;
            Researcher = researcher;
            StudyId = studyId;
            StudyName = studyName;
            Study = study;
        }
    }
}
