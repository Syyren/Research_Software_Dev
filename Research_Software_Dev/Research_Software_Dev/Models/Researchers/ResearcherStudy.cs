using Research_Software_Dev.Models.Studies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherStudy(int r_id, int s_id, string studyName)
    {
        //Composite key auto generated in context

        //Researcher FK
        [Required]
        [ForeignKey("Researcher")]
        public int ResearcherId { get; set; } = r_id;

        [Required]
        public Researcher Researcher { get; set; }

        //Study FK
        [Required]
        [ForeignKey("Study")]
        public int StudyId { get; set; } = s_id;
        [Required]
        public string StudyName { get; set; } = studyName;
        [Required]
        public Study Study { get; set; }
    }
}
