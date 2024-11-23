using System;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Models.Participants
{
    public class Participant(int id, string firstName, string lastName, string address, string email, string phoneNumber)
    {
        [Key]
        public required int ParticipantId { get; set; } = id;

        [Required]
        [StringLength(50)]
        public required string ParticipantFirstName { get; set; } = firstName;

        [Required]
        [StringLength(50)]
        public required string ParticipantLastName { get; set; } = lastName;

        [StringLength(255)]
        public string? ParticipantAddress { get; set; } = address;

        [StringLength(50)]
        [EmailAddress]
        public string? ParticipantEmail { get; set; } = email;

        [Phone]
        public string? ParticipantPhoneNumber { get; set; } = phoneNumber;
    }
}
