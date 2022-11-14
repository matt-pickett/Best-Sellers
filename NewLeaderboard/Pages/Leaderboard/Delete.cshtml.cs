using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NewLeaderboard.Data;
using NewLeaderboard.Models;

namespace NewLeaderboard.Pages.Leaderboard
{
    public class DeleteModel : PageModel
    {
        private readonly NewLeaderboard.Data.NewLeaderboardContext _context;

        public DeleteModel(NewLeaderboard.Data.NewLeaderboardContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Author AuthorObj { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FirstOrDefaultAsync(m => m.AuthorID == id);

            if (author == null)
            {
                return NotFound();
            }
            else 
            {
                AuthorObj = author;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }
            var author = await _context.Author.FindAsync(id);

            if (author != null)
            {
                AuthorObj = author;
                _context.Author.Remove(AuthorObj);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
