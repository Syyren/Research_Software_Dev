using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Sessions;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Services;

namespace Research_Software_Dev.Pages.Sessions
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;
        private readonly string[] permissions = { "Study Admin", "High-Auth", "Mid-Auth" };

        public DetailsModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Session Session { get; set; } = default!;
        public List<Participant>? Participants { get; set; } = new List<Participant>();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            //Security Check 1
            if (!Helper.IsLoggedIn(_userManager, User)) return RedirectToPage("/NotFound");
            if (!Helper.IsAuthorized(User, permissions)) return Forbid();

            //null id check
            if (id == null) return RedirectToPage("/NotFound");

            // Fetch session details
            var researcherId = _userManager.GetUserId(User);
            var session = await _context.Sessions
                .Include(s => s.Study)
                .FirstOrDefaultAsync(m => m.SessionId == id &&
                    _context.ResearcherSessions.Any(rs => rs.SessionId == id && rs.ResearcherId == researcherId));
            //fetching participants
            var participants = await _context.ParticipantSessions
                .Where(ps => ps.SessionId == id)
                .Select(ps => ps.Participant)
                .ToListAsync();


            if (session == null)
            {
                return RedirectToPage("/NotFound");
            }

            Participants = participants;
            Session = session;
            return Page();
        }
    }
}
