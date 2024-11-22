namespace Research_Software_Dev.Models.Researchers
{
    public class ResearcherStudy(int r_id, int s_id, string studyName)
    {
        public int ResearcherStudyId { get; } = int.Parse($"{r_id}{s_id}");

        //Researcher FK
        public int ResearcherId { get; set; } = r_id;

        //Study FK
        public int StudyId { get; set; } = s_id;
        public string StudyName { get; set; } = studyName;
    }
}
