using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherStudy
    {
        public ResearcherStudy(int r_id, int s_id)
        {
            ResearcherId = r_id;
            StudyId = s_id;
            ResearcherStudyId = int.Parse($"{ResearcherId}{StudyId}");
        }
        public int ResearcherStudyId { get; }

        //Researcher FK
        public int ResearcherId { get; set; }

        //Study FK
        public int StudyId { get; set; }
    }
}
