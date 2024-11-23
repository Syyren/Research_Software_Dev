using Research_Software_Dev.Models.Studies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherStudy
    {
        //Composite key auto generated in context

        [Required]
        [ForeignKey("Researcher")]
        public int ResearcherId { get; set; }

        [Required]
        public Researcher Researcher { get; set; }


        [Required]
        [ForeignKey("Study")]
        public int StudyId { get; set; }

        [Required]
        public Study Study { get; set; }
    }
}
