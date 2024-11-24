using System;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Participants
{
    public class Participant
    {
        [Key]
        public required int ParticipantId { get; set; }

        [Required]
        [StringLength(50)]
        public required string ParticipantFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string ParticipantLastName { get; set; }

        [StringLength(255)]
        public string? ParticipantAddress { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string? ParticipantEmail { get; set; }

        [Phone]
        public string? ParticipantPhoneNumber { get; set; }

        //Constructors
        public Participant() { }
        public Participant(int participantId, string participantFirstName, string participantLastName, string? participantAddress, string? participantEmail, string? participantPhoneNumber)
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
