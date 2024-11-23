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
        [ForeignKey("Study")]
        public int StudyId { get; set; }

        [Required]
        public Study Study { get; set; }


    }
}
