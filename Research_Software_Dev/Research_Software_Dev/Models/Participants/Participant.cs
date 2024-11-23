using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Participants
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }

        [Required]
        [StringLength(50)]
        public string ParticipantFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string ParticipantLastName { get; set; }

        [Required]
        [StringLength(255)]
        public string ParticipantAddress { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string ParticipantEmail { get; set; }

        [Required]
        [StringLength(50)]
        [Phone]
        public string ParticipantPhoneNumber { get; set; }
    }
}
