using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BestSellers.Data;
using BestSellers.Models;

namespace BestSellers.Pages.Leaderboard
{
    public class DetailsModel : PageModel
    {
        private readonly BestSellers.Data.BestSellersContext _context;

        public DetailsModel(BestSellers.Data.BestSellersContext context)
        {
            _context = context;
        }

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
    }
}
