using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;

namespace Research_Software_Dev.Pages.ParticipantSessions
{
    public class EditModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public EditModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ParticipantSession ParticipantSession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantsession =  await _context.ParticipantSessions.FirstOrDefaultAsync(m => m.ParticipantId == id);
            if (participantsession == null)
            {
                return NotFound();
            }
            ParticipantSession = participantsession;
           ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "ParticipantId");
           ViewData["SessionId"] = new SelectList(_context.Sessions, "SessionId", "SessionId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ParticipantSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantSessionExists(ParticipantSession.ParticipantId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ParticipantSessionExists(string id)
        {
            return _context.ParticipantSessions.Any(e => e.ParticipantId == id);
        }
    }
}
