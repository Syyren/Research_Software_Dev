using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Research_Software_Dev.Models.Participants
{
    public class Participant
    {
        public required int ParticipantId { get; set; }

        [Required]
        public required string ParticipantFirstName { get; set; }

        [Required]
        public required string ParticipantLastName { get; set; }

        public string? ParticipantAddress { get; set; }

        [EmailAddress]
        public string? ParticipantEmail { get; set; }
        public string? ParticipantPhoneNumber { get; set; }
    }
}
