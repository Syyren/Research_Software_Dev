using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Pages.Sessions
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public DeleteModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Session Session { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Verify permissions
            var roles = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth"))
            {
                return Forbid();
            }

            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            var session = await _context.Sessions.FirstOrDefaultAsync(m => m.SessionId == id);

            if (session == null)
            {
                return RedirectToPage("/NotFound");
            }
            else
            {
                Session = session;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            // Verify permissions
            var roles = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth"))
            {
                return Forbid();
            }

            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                // Remove related ResearcherSessions
                var researcherSessions = _context.ResearcherSessions.Where(rs => rs.SessionId == id);
                _context.ResearcherSessions.RemoveRange(researcherSessions);

                // Remove related ParticipantSessions
                var participantSessions = _context.ParticipantSessions.Where(ps => ps.SessionId == id);
                _context.ParticipantSessions.RemoveRange(participantSessions);

                Session = session;
                _context.Sessions.Remove(Session);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
