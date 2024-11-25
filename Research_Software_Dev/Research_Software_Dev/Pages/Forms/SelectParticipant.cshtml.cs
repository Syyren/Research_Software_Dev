using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
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

        public async Task OnGetAsync()
        {
            Participants = await _context.Participants
                .OrderBy(p => p.ParticipantLastName)
                .ThenBy(p => p.ParticipantFirstName)
                .ToListAsync();
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
