using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BestSellers.Data;
using BestSellers.Models;

namespace BestSellers.Pages.Leaderboard
{
    public class CreateModel : PageModel
    {
        private readonly BestSellers.Data.BestSellersContext _context;

        public CreateModel(BestSellers.Data.BestSellersContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Author AuthorObj { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Author.Add(AuthorObj);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
