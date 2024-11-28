using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Models.Researchers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Admin
{
    [Authorize(Roles = "Study Admin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<Researcher> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<Researcher> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Researcher User { get; set; }
        public List<string> AllRoles { get; set; } = new();
        public List<string> UserRoles { get; set; } = new();

        [BindProperty]
        public List<string> SelectedRoles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            User = await _userManager.FindByIdAsync(id);
            if (User == null) return RedirectToPage("/NotFound");

            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            UserRoles = (await _userManager.GetRolesAsync(User)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            User = await _userManager.FindByIdAsync(id);
            if (User == null) return RedirectToPage("/NotFound");

            var currentRoles = await _userManager.GetRolesAsync(User);
            var rolesToAdd = SelectedRoles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(SelectedRoles).ToList();

            await _userManager.AddToRolesAsync(User, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(User, rolesToRemove);

            return RedirectToPage("Index");
        }
    }
}