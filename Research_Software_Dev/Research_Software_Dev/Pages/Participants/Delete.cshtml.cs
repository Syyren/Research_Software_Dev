using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;

namespace Research_Software_Dev.Pages.Participants
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Participant Participant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }
            //verifies ownership before deleting
            var participantStudy = await _context.ParticipantStudies
                .FirstOrDefaultAsync(ps => ps.ParticipantId == Participant.ParticipantId &&
                    _context.ResearcherStudies.Any(rs => rs.StudyId == ps.StudyId && rs.ResearcherId == researcherId));
            if (participantStudy == null)
            {
                return NotFound();
            }
            Participant = participantStudy.Participant;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            //verifies ownership before deleting
            var participantStudy = await _context.ParticipantStudies
                .FirstOrDefaultAsync(ps => ps.ParticipantId == Participant.ParticipantId &&
                    _context.ResearcherStudies.Any(rs => rs.StudyId == ps.StudyId && rs.ResearcherId == researcherId));

            if (participantStudy == null)
            {
                return NotFound();
            }

            //remove the association from ParticipantStudies
            _context.ParticipantStudies.Remove(participantStudy);

            //remove the participant
            var otherAssociationsExist = await _context.ParticipantStudies.AnyAsync(ps => ps.ParticipantId == id);
            if (!otherAssociationsExist)
            {
                var participant = await _context.Participants.FindAsync(id);
                if (participant != null)
                {
                    _context.Participants.Remove(participant);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
