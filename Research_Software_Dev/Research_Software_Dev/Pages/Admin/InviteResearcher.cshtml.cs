using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;
using Research_Software_Dev.Services.EmailService;
using System.ComponentModel.DataAnnotations;

namespace Research_Software_Dev.Pages.Admin
{
    public class InviteResearcherModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;
        private readonly IEmail _emailService;
        private readonly string[] permissions = { "Study Admin", "High-Auth" };

        public InviteResearcherModel(ApplicationDbContext context, UserManager<Researcher> userManager, IEmail emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        public string ReceiverEmail { get; set; }

        // dropdown list of studies
        public List<Study> Studies { get; set; } = new List<Study>();

        //Bind StudyId from the dropdown
        [BindProperty]
        [Required(ErrorMessage = "A Study is required.")]
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

            // Check authorization - only Study Admins or High-Auth users can create participants
            var roles = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Any(role => permissions.Contains(role)))
            {
                return Forbid();
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

            //make invitation message
            var subject = $"You have been invited to the study: {SelectedStudyId}";
            var message = $"Hello,\n\nYou have been invited to join the study '{SelectedStudyId}'. Please click the link below to complete your registration.\n\n" +
                          $"[Registration Link - Expiry: 24 hours]({Url.Page("/RegisterPage", new { token = Guid.NewGuid() })})";

            //send email
            await _emailService.SendEmailAsync(ReceiverEmail, subject, message);

            return RedirectToPage("./Index");
        }
    }
}
