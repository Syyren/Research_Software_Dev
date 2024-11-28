using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;

namespace Research_Software_Dev.Pages.ParticipantSessions
{
    public class DetailsModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public DetailsModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ParticipantSession ParticipantSession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            var participantsession = await _context.ParticipantSessions.FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participantsession == null)
            {
                return RedirectToPage("/NotFound");
            }
            else
            {
                ParticipantSession = participantsession;
            }
            return Page();
        }
    }
}
