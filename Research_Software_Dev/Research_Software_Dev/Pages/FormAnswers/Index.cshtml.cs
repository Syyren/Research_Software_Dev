using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize]
    public class SelectParticipantModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SelectParticipantModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Participant> Participants { get; set; }

        [BindProperty]
        public string ParticipantId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (roles.Contains("Study Admin") || roles.Contains("High-Auth"))
            {
                Participants = await _context.Participants
                    .OrderBy(p => p.ParticipantLastName)
                    .ThenBy(p => p.ParticipantFirstName)
                    .ToListAsync();
            }
            else if (roles.Contains("Mid-Auth"))
            {
                Participants = await _context.Participants
                    .Select(p => new Participant
                    {
                        ParticipantId = p.ParticipantId,
                        ParticipantFirstName = p.ParticipantFirstName,
                        ParticipantLastName = p.ParticipantLastName
                    })
                    .OrderBy(p => p.ParticipantLastName)
                    .ThenBy(p => p.ParticipantFirstName)
                    .ToListAsync();
            }
            else
            {
                return Forbid();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(ParticipantId))
            {
                ModelState.AddModelError(string.Empty, "Please select a participant.");
                return Page();
            }

            return RedirectToPage("./SelectSession", new { participantId = ParticipantId });
        }
    }
}
