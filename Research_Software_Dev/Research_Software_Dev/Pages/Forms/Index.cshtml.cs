using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize(Roles = "Low-Auth,Mid-Auth,High-Auth,Study Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Form> Forms { get; set; }

        public async Task OnGetAsync()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (roles.Contains("Study Admin") || roles.Contains("High-Auth") || roles.Contains("Mid-Auth") || roles.Contains("Low-Auth") || roles.Contains("Researcher"))
            {
                Forms = await _context.Forms
                    .OrderBy(f => f.FormName) // Sort forms alphabetically by FormName
                    .ToListAsync();
            }
            else
            {
                Forms = new List<Form>();
            }
        }
    }
}
