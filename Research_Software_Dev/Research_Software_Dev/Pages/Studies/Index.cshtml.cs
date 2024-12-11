using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Studies
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Researcher> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(UserManager<Researcher> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public List<Study> Study { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //gets the current user's ID
            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                //if no user is logged in, redirect or show a message as needed
                Study = new List<Study>();
                return RedirectToPage("/NotFound"); ;
            }

            //fetches studies associated with the current user
            Study = await _dbContext.ResearcherStudies
                .Where(rs => rs.ResearcherId == researcherId)
                .Include(rs => rs.Study)
                .Select(rs => rs.Study)
                .OrderBy(rs => rs.StudyName)
                .ToListAsync();
            return Page();
        }
    }
}