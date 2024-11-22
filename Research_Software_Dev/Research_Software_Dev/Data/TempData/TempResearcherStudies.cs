using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Data.TempData;

namespace Research_Software_Dev.Data.TempData
{
    public class TempResearcherStudies
    {
        public static TempResearcher temp_researcher;
        public static TempStudies studies;
        public readonly ResearcherStudy researcher_study1 = new(temp_researcher.researcher1.ResearcherId, studies.study1.StudyId);
        public readonly ResearcherStudy researcher_study2 = new(temp_researcher.researcher1.ResearcherId, studies.study2.StudyId);
        public readonly ResearcherStudy researcher_study3 = new(temp_researcher.researcher1.ResearcherId, studies.study3.StudyId);
        public readonly ResearcherStudy researcher_study4 = new(temp_researcher.researcher1.ResearcherId, studies.study4.StudyId);
    }
}
