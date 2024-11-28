using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class DetailsModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public DetailsModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Session Session { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Verify permissions
            var roles = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth") && !roles.Contains("Mid-Auth"))
            {
                return Forbid();
            }

            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            // Fetch session details and verify access
            var researcherId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var session = await _context.Sessions
                .FirstOrDefaultAsync(m => m.SessionId == id &&
                    _context.ResearcherSessions.Any(rs => rs.SessionId == id && rs.ResearcherId == researcherId));

            if (session == null)
            {
                return RedirectToPage("/NotFound");
            }

            Session = session;
            return Page();
        }
    }
}
