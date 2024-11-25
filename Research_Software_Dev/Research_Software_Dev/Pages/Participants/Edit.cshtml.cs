using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Pages.Participants
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Participant Participant { get; set; } = default!;

        [BindProperty]
        public string StudyName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            //get the logged-in researcher's ID
            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return Unauthorized();
            }

            //fetch participant and verify the logged-in researcher is associated with the study
            var participantStudy = await _context.ParticipantStudies
                .Include(ps => ps.Participant)
                .Include(ps => ps.Study) //include Study for StudyName
                .FirstOrDefaultAsync(ps => ps.ParticipantId == id &&
                    _context.ResearcherStudies.Any(rs => rs.StudyId == ps.StudyId && rs.ResearcherId == researcherId));

            if (participantStudy == null)
            {
                return RedirectToPage("/NotFound");
            }

            Participant = participantStudy.Participant;
            StudyName = participantStudy.Study.StudyName;
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

            //gets the logged-in user's ID
            var researcherId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            //verifies ownership before updating
            var participantStudy = await _context.ParticipantStudies
                .FirstOrDefaultAsync(ps => ps.ParticipantId == Participant.ParticipantId &&
                    _context.ResearcherStudies.Any(rs => rs.StudyId == ps.StudyId && rs.ResearcherId == researcherId));

            if (participantStudy == null)
            {
                return RedirectToPage("/NotFound");
            }

            //attaches and modifies the study
            _context.Attach(Participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(Participant.ParticipantId))
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

        private bool ParticipantExists(string id)
        {
            return _context.Participants.Any(e => e.ParticipantId == id);
        }
    }
}
