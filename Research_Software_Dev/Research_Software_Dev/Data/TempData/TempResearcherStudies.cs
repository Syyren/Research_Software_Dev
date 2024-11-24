using Research_Software_Dev.Models.Researchers;

namespace Research_Software_Dev.Data.TempData
{
    public class TempResearcherStudies(TempResearcher temp_researcher, TempStudies studies)
    {
        public ResearcherStudy researcher_study1 = new(temp_researcher.researcher1.Id, studies.study1.StudyId, studies.study1.StudyName);
        public ResearcherStudy researcher_study2 = new(temp_researcher.researcher1.Id, studies.study2.StudyId, studies.study2.StudyName);
        public ResearcherStudy researcher_study3 = new(temp_researcher.researcher1.Id, studies.study3.StudyId, studies.study3.StudyName);
        public ResearcherStudy researcher_study4 = new(temp_researcher.researcher1.Id, studies.study4.StudyId, studies.study4.StudyName);
    }
}
