using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Pages.Sessions
{
    public class IndexModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public IndexModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Session> Session { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Session = await _context.Sessions
                .Include(s => s.Study).ToListAsync();
        }
    }
}
