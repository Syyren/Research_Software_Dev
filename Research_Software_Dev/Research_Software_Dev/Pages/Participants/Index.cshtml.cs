using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;

namespace Research_Software_Dev.Pages.Participants
{
    public class IndexModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public IndexModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Participant> Participant { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Participant = await _context.Participants.ToListAsync();
        }
    }
}
