using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Sessions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Research_Software_Dev.Pages.Sessions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Session> Sessions { get; set; } = new List<Session>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the current logged-in user's ID
            var userId = _userManager.GetUserId(User);

            // Ensure the user is logged in
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge(); // Redirects to login if user is not authenticated
            }

            // Step 1: Get session IDs from ResearcherSessions for the logged-in user
            var sessionIds = await _context.ResearcherSessions
                .Where(ps => ps.ResearcherId == userId)
                .Select(ps => ps.SessionId)
                .ToListAsync();

            // Step 2: Query Sessions table using the session IDs
            Sessions = await _context.Sessions
                .Where(s => sessionIds.Contains(s.SessionId))
                .Include(s => s.Study)
                .ToListAsync();

            return Page();
        }
    }
}
