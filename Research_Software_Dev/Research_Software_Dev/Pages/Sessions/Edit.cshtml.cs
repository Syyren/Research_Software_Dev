using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Pages.Sessions
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public EditModel(Research_Software_Dev.Data.ApplicationDbContext context)
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
            Session = session;
            ViewData["StudyId"] = new SelectList(_context.Studies, "StudyId", "StudyId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(Session.SessionId))
                {
                    return RedirectToPage("/NotFound");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SessionExists(string id)
        {
            return _context.Sessions.Any(e => e.SessionId == id);
        }
    }
}
