using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Participant Participant { get; set; }

        // dropdown list of studies
        public List<Study> Studies { get; set; } = new List<Study>();

        //Bind StudyId from the dropdown
        [BindProperty]
        [Required(ErrorMessage="A Study is Required")]
        public string SelectedStudyId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //gets the logged-in user's ID (ResearcherId)
            var researcherId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            // Fetch studies associated with the researcher
            Studies = await _context.ResearcherStudies
                .Where(rs => rs.ResearcherId == researcherId)
                .Include(rs => rs.Study)
                .Select(rs => rs.Study)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            //gets the logged-in user's ID (ResearcherId)
            var researcherId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            if (!ModelState.IsValid)
            {
                //reload dropdown list in event of a validation error
                Studies = await _context.ResearcherStudies
                    .Where(rs => rs.ResearcherId == researcherId)
                    .Include(rs => rs.Study)
                    .Select(rs => rs.Study)
                    .ToListAsync();
                return Page();
            }

            //researcher must have selected study matching in researcher studies
            var researcherStudy = await _context.ResearcherStudies
                .FirstOrDefaultAsync(rs => rs.ResearcherId == researcherId && rs.StudyId == SelectedStudyId);

            if (researcherStudy == null)
            {
                //if researcher not associated with selected study, error
                ModelState.AddModelError("SelectedStudyId", "You are not associated with this study.");
                Studies = await _context.ResearcherStudies
                    .Where(rs => rs.ResearcherId == researcherId)
                    .Include(rs => rs.Study)
                    .Select(rs => rs.Study)
                    .ToListAsync();
                return Page();
            }


            //generates a unique ParticipantId using GUID
            Participant.ParticipantId = Guid.NewGuid().ToString();

            //creates a ResearcherStudy entry to associate the study with the researcher
            var participantStudy = new ParticipantStudy
            {
                ParticipantId = Participant.ParticipantId,
                StudyId = SelectedStudyId,
            };

            //adds the new participant and participant-study link to the database
            _context.Participants.Add(Participant); // Add the Participant
            _context.ParticipantStudies.Add(participantStudy); // Add the ParticipantStudy link
            await _context.SaveChangesAsync(); // Save to the database

            return RedirectToPage("./Index");
        }
    }
}
