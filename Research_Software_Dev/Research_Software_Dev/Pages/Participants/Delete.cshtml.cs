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
        private readonly string[] pageauth = { "Study Admin", "High-Auth" };

        public DeleteModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Participant Participant { get; set; } = default!;

        [BindProperty]
        public bool DeleteFromAllStudies { get; set; } = false;

        [BindProperty]
        public string StudyName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //get the logged-in researcher's ID
            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return Unauthorized();
            }

            // Fetch roles and verify permissions
            var roles = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Any(role => pageauth.Contains(role)))
            {
                return Forbid();
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

            // Fetch roles and verify permissions
            var roles = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Any(role => pageauth.Contains(role)))
            {
                return Forbid();
            }

            //get and verifies ownership before updating
            var participantStudy = await _context.ParticipantStudies
                .FirstOrDefaultAsync(ps => ps.ParticipantId == id &&
                    _context.ResearcherStudies.Any(rs => rs.StudyId == ps.StudyId && rs.ResearcherId == researcherId));

            if (participantStudy == null)
            {
                return NotFound();
            }

            //If Participant is being removed from all studies via checkbox
            if (DeleteFromAllStudies)
            {
                //get participant from all studies they are in
                var participantStudies = await _context.ParticipantStudies
                    .Where(ps => ps.ParticipantId == id)
                    .ToListAsync();

                //if participant in any studies => remove
                if (participantStudies.Any())
                {
                    //Remove from all studies
                    _context.ParticipantStudies.RemoveRange(participantStudies);
                }

                //remove participant
                var participant = await _context.Participants.FindAsync(id);
                if (participant != null)
                {
                    _context.Participants.Remove(participant);
                }
            }
            else
            {
                //remove from ParticipantStudies
                _context.ParticipantStudies.Remove(participantStudy);

                //remove participant
                var participant = await _context.Participants.FindAsync(id);
                if (participant != null)
                {
                    _context.Participants.Remove(participant);
                }
            }

            //save to db
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
