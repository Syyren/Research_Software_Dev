namespace Research_Software_Dev.Models.Studies
{
    public class Study(int id, string name, string description)
    {
        public int StudyId { get; set; } = id;
        public string StudyName { get; set; } = name;
        public string StudyDescription { get; set; } = description;
    }

}
