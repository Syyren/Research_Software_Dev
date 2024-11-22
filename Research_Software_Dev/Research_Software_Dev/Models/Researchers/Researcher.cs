using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Models.Researchers
{
    public class Researcher(int id, string first_name, string last_name, string address, string email, string phone_number, string password)
    {
        public int ResearcherId { get; set; } = id;
        public string ResearcherFirstName { get; set; } = first_name;
        public string ResearcherLastName { get; set; } = last_name;
        public string ResearcherAddress { get; set; } = address;
        public string ResearcherEmail { get; set; } = email;
        public string ResearcherPhoneNumber { get; set; } = phone_number;
        public string ResearcherPassword { get; set; } = password;
    }
}
