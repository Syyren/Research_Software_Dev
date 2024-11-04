using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Models.Sessions
{
    public class Session
    {
        public int SessionId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeEnd { get; set; }

        //Study Fk
        public int StudyId { get; set; }
        public Study Study { get; set; }


    }
}
