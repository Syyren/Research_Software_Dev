using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;

namespace Research_Software_Dev.Pages.Participants
{
    public class DetailsModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public DetailsModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Participant Participant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Participants.FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participant == null)
            {
                return NotFound();
            }
            else
            {
                Participant = participant;
            }
            return Page();
        }
    }
}
