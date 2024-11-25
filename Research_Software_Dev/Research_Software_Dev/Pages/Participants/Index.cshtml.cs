using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Pages.Participants
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ParticipantViewModel> Participant { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //gets the current user's ID
            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound"); ;
            }

            //fetches Participants associated with the current Researcher through Studies
            Participant = await _context.ParticipantStudies
            .Include(ps => ps.Participant)
            .Include(ps => ps.Study)
            .Where(ps => _context.ResearcherStudies
                .Any(rs => rs.StudyId == ps.StudyId && rs.ResearcherId == researcherId))
            .Select(ps => new ParticipantViewModel
            {
                ParticipantId = ps.Participant.ParticipantId,
                ParticipantFirstName = ps.Participant.ParticipantFirstName,
                ParticipantLastName = ps.Participant.ParticipantLastName,
                ParticipantAddress = ps.Participant.ParticipantAddress,
                ParticipantEmail = ps.Participant.ParticipantEmail,
                ParticipantPhoneNumber = ps.Participant.ParticipantPhoneNumber,
                StudyName = ps.Study.StudyName
            })
            .ToListAsync();

            return Page();
        }
    }
    public class ParticipantViewModel
    {
        public string ParticipantId { get; set; }
        public string ParticipantFirstName { get; set; }
        public string ParticipantLastName { get; set; }
        public string? ParticipantAddress { get; set; }
        public string? ParticipantEmail { get; set; }
        public string? ParticipantPhoneNumber { get; set; }
        public string StudyName { get; set; }
    }
}
