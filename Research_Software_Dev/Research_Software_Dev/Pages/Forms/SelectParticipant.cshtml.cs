using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Research_Software_Dev.Pages.Forms
{
    public class SelectParticipantModel : PageModel
    {
        [BindProperty]
        public string FormId { get; set; }

        [BindProperty]
        public string ParticipantSessionId { get; set; }

        public Dictionary<string, string> ParticipantSessions { get; set; }

        public IActionResult OnGet(string formId)
        {
            FormId = formId;

            // Placeholder participant sessions
            ParticipantSessions = new Dictionary<string, string>
            {
                { "1", "Session A" },
                { "2", "Session B" },
                { "3", "Session C" }
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./SelectForm", new { formId = FormId, participantSessionId = ParticipantSessionId });
        }
    }
}