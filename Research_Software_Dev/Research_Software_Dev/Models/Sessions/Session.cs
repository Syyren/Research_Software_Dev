using Research_Software_Dev.Models.Studies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Sessions
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeOnly TimeStart { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeOnly TimeEnd { get; set; }

        //Study Fk
        [Required]
        public int StudyId { get; set; }
        [ForeignKey("StudyId")]
        public Study Study { get; set; }

        //Constructors
        public Session(){ }
        public Session(int id, DateOnly date, TimeOnly timeStart, TimeOnly timeEnd, int studyId)
        {
            SessionId = id;
            Date = date;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            StudyId = studyId;
        }


    }
}
