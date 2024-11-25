﻿using System;
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
    public class DeleteModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public DeleteModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Session Session { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FirstOrDefaultAsync(m => m.SessionId == id);

            if (session == null)
            {
                return NotFound();
            }
            else
            {
                Session = session;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                Session = session;
                _context.Sessions.Remove(Session);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
