using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;

namespace Research_Software_Dev.Pages.ParticipantSessions
{
    public class IndexModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public IndexModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ParticipantSession> ParticipantSession { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ParticipantSession = await _context.ParticipantSessions
                .Include(p => p.Participant)
                .Include(p => p.Session).ToListAsync();
        }
    }
}
