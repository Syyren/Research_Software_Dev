using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherStudy
    {
        // Composite key auto-generated in the context

        // Researcher FK
        [Required]
        public string ResearcherId { get; set; }

        [ForeignKey("ResearcherId")]
        public Researcher Researcher { get; set; }  // Navigation Property

        // Study FK
        [Required]
        public string StudyId { get; set; }

        [ForeignKey("StudyId")]
        public Study Study { get; set; }  // Navigation Property

        // Constructors
        public ResearcherStudy() { }

        public ResearcherStudy(string researcherId, string studyId)
        {
            ResearcherId = researcherId;
            StudyId = studyId;
        }
    }
}
