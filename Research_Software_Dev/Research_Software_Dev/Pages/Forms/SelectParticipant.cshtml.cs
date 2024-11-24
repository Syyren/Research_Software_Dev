using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Research_Software_Dev.Pages.Forms
{
    public class SelectParticipantModel : PageModel
    {
        [BindProperty]
        public int FormId { get; set; }

        [BindProperty]
        public int ParticipantSessionId { get; set; }

        public Dictionary<int, string> ParticipantSessions { get; set; }

        public IActionResult OnGet(int formId)
        {
            FormId = formId;

            // Placeholder participant sessions
            ParticipantSessions = new Dictionary<int, string>
            {
                { 1, "Session A" },
                { 2, "Session B" },
                { 3, "Session C" }
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./SelectForm", new { participantSessionId = ParticipantSessionId });
        }
    }
}