using Research_Software_Dev.Models.Studies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Sessions
{
    public class Session(int id, DateOnly date, TimeOnly timeStart, TimeOnly timeEnd, int studyId)
    {
        [Key]
        public int SessionId { get; set; } = id;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; } = date;

        [Required]
        [DataType(DataType.Time)]
        public TimeOnly TimeStart { get; set; } = timeStart;

        [Required]
        [DataType(DataType.Time)]
        public TimeOnly TimeEnd { get; set; } = timeEnd;

        //Study Fk
        [Required]
        [ForeignKey("Study")]
        public int StudyId { get; set; } = studyId;

        [Required]
        public Study Study { get; set; }


    }
}
