using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Pages.Participants
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ParticipantViewModel> Participant { get; set; } = new List<ParticipantViewModel>();

        public List<Study> Studies { get; set; } = new List<Study>();

        [BindProperty(SupportsGet = true)]
        public string SelectedStudyId { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            //gets the current user's ID
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

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth") && !roles.Contains("Mid-Auth"))
            {
                return Forbid();
            }

            //get studies the researcher is part of
            Studies = await _context.ResearcherStudies
                .Where(rs => rs.ResearcherId == researcherId)
                .Include(rs => rs.Study)
                .Select(rs => rs.Study)
                .ToListAsync();

            //get participants and associated studies, filter if study selected
            var query = _context.ParticipantStudies
                .Include(ps => ps.Participant)
                .Include(ps => ps.Study)
                .Where(ps => _context.ResearcherStudies
                    .Any(rs => rs.StudyId == ps.StudyId && rs.ResearcherId == researcherId));

            if (!string.IsNullOrEmpty(SelectedStudyId))
            {
                query = query.Where(ps => ps.StudyId == SelectedStudyId);
            }

            Participant = await query
                .Select(ps => new ParticipantViewModel
                {
                    ParticipantId = ps.Participant.ParticipantId,
                    ParticipantFirstName = ps.Participant.ParticipantFirstName,
                    ParticipantLastName = ps.Participant.ParticipantLastName,
                    ParticipantAddress = ps.Participant.ParticipantAddress,
                    ParticipantEmail = ps.Participant.ParticipantEmail,
                    ParticipantPhoneNumber = ps.Participant.ParticipantPhoneNumber,
                    StudyName = ps.Study.StudyName
                })
                .ToListAsync();

            return Page();
        }
    }

    public class ParticipantViewModel
    {
        public string ParticipantId { get; set; }
        [Display(Name = "First Name")]
        public string ParticipantFirstName { get; set; }
        [Display(Name = "Last Name")]
        public string ParticipantLastName { get; set; }
        [Display(Name = "Address")]
        public string? ParticipantAddress { get; set; }
        [Display(Name = "Email")]
        public string? ParticipantEmail { get; set; }
        [Display(Name = "Phone Number")]
        public string? ParticipantPhoneNumber { get; set; }
        [Display(Name = "Study Name")]
        public string StudyName { get; set; }
    }
}
