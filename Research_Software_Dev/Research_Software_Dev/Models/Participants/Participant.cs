using System;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Participants
{
    public class Participant
    {
        [Key]
        public required string ParticipantId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public required string ParticipantFirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public required string ParticipantLastName { get; set; }

        [StringLength(255)]
        [Display(Name = "Address")]
        public string? ParticipantAddress { get; set; }

        [StringLength(50)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? ParticipantEmail { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string? ParticipantPhoneNumber { get; set; }

        //Constructors
        public Participant() { }
        public Participant(string participantId, string participantFirstName, string participantLastName, 
            string? participantAddress, string? participantEmail, string? participantPhoneNumber)
        {
            ParticipantId = participantId;
            ParticipantFirstName = participantFirstName;
            ParticipantLastName = participantLastName;
            ParticipantAddress = participantAddress;
            ParticipantEmail = participantEmail;
            ParticipantPhoneNumber = participantPhoneNumber;
        }
    }
}
