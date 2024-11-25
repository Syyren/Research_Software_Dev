using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Models.Researchers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Admin
{
    [Authorize(Roles = "Study Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<Researcher> _userManager;

        public IndexModel(UserManager<Researcher> userManager)
        {
            _userManager = userManager;
        }

        public List<Researcher> Users { get; set; } = new();
        public Dictionary<string, List<string>> UserRoles { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
            UserRoles = new Dictionary<string, List<string>>();

            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserRoles[user.Id] = roles.ToList();
            }
        }
    }
}
