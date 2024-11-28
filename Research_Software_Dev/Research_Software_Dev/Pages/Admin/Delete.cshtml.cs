using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Models.Researchers;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Admin
{
    [Authorize(Roles = "Study Admin")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<Researcher> _userManager;

        public DeleteModel(UserManager<Researcher> userManager)
        {
            _userManager = userManager;
        }

        public Researcher User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            User = await _userManager.FindByIdAsync(id);
            if (User == null) return RedirectToPage("/NotFound");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            User = await _userManager.FindByIdAsync(id);
            if (User == null) return RedirectToPage("/NotFound");

            await _userManager.DeleteAsync(User);
            return RedirectToPage("Index");
        }
    }
}
