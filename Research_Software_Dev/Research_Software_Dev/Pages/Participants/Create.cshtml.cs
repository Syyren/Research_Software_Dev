using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using Research_Software_Dev.Services;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Pages.Participants
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;
        private readonly string[] permissions = { "Study Admin", "High-Auth" };

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
        [Required(ErrorMessage = "A Study is Required")]
        public string SelectedStudyId { get; set; }


        //GET
        public async Task<IActionResult> OnGetAsync()
        {
            //Security Check 1
            if (!Helper.IsLoggedIn(_userManager, User)) return RedirectToPage("/NotFound");
            if (!Helper.IsAuthorized(User, permissions)) return Forbid();

            //fetch studies associated with the researcher
            var researcherId = _userManager.GetUserId(User);
            Studies = await Helper.GetStudiesForResearcher(_context, researcherId);

            return Page();
        }


        //POST
        public async Task<IActionResult> OnPostAsync()
        {
            //Security Check 1
            if (!Helper.IsLoggedIn(_userManager, User)) return RedirectToPage("/NotFound");
            if (!Helper.IsAuthorized(User, permissions)) return Forbid();

            var researcherId = _userManager.GetUserId(User);
            //model validation
            if (!ModelState.IsValid)
            {
                Studies = await Helper.GetStudiesForResearcher(_context, researcherId);
                return Page();
            }

            //Security Check 2
            //is researcher in study
            if (!await Helper.IsResearcherInStudy(_context, researcherId, SelectedStudyId))
            {
                Studies = await Helper.GetStudiesForResearcher(_context, researcherId);
                return Page();
            }


            //check participant doubles in study
            var isParticipantInStudy = await Helper.IsParticipantInStudyByName(
                _context,
                Participant.ParticipantFirstName,
                Participant.ParticipantLastName, SelectedStudyId
            );
            if (isParticipantInStudy)
            {
                ModelState.AddModelError(string.Empty, "A participant with the same first and last name already exists in this study. Please add an identifier to the name if this is a separate person.");
                Studies = await Helper.GetStudiesForResearcher(_context, researcherId);
                return Page();
            }

            //get a GUID for Participant
            Participant.ParticipantId = Helper.GetGUID();

            //creates a ParticipantStudy entry to associate the study with the participant
            var participantStudy = new ParticipantStudy(Participant.ParticipantId, SelectedStudyId);

            //add participant and participantstudy to db
            _context.Participants.Add(Participant);
            _context.ParticipantStudies.Add(participantStudy);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}