// Edit doesn't work - error on line 51 'ClaimsDetails' not found

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewLeaderboard.Data;
using NewLeaderboard.Models;

namespace NewLeaderboard.Pages.Leaderboard
{
    public class EditModel : PageModel
    {
        private readonly NewLeaderboard.Data.NewLeaderboardContext _context;

        public EditModel(NewLeaderboard.Data.NewLeaderboardContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Author AuthorObj { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author =  await _context.Author.FirstOrDefaultAsync(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }
            AuthorObj = author;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AuthorObj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(AuthorObj.AuthorID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserExists(int id)
        {
          return _context.Author.Any(e => e.AuthorID == id);
        }
    }
}
