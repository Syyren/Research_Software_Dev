using Research_Software_Dev.Models.Studies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Sessions
{
    public class Session
    {
        [Key]
        public string SessionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Session Date")]
        public DateOnly Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Session Start Time")]
        public TimeOnly TimeStart { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Session End Time")]
        public TimeOnly TimeEnd { get; set; }

        //Study Fk
        [Required]
        [Display(Name = "Which Study is this a Session for?")]
        public string StudyId { get; set; }
        [ForeignKey("StudyId")]
        public Study? Study { get; set; }

        //Constructors
        public Session(){ }
        public Session(string id, DateOnly date, TimeOnly timeStart, TimeOnly timeEnd, string studyId)
        {
            SessionId = id;
            Date = date;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            StudyId = studyId;
        }


    }
}
