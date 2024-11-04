using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherStudy
    {
        public ResearcherStudy()
        {
            ResearcherStudyId = int.Parse($"{ResearcherId}{StudyId}");
        }
        public int ResearcherStudyId { get; }

        //Researcher FK
        public int ResearcherId { get; set; }
        public Researcher Researcher { get; set; }

        //Study FK
        public int StudyId { get; set; }
        public Study Study { get; set; }
    }
}
