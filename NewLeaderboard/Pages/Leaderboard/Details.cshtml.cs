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
    public class DetailsModel : PageModel
    {
        private readonly NewLeaderboard.Data.NewLeaderboardContext _context;

        public DetailsModel(NewLeaderboard.Data.NewLeaderboardContext context)
        {
            _context = context;
        }

      public User UserObj { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }
            else 
            {
                UserObj = user;
            }
            return Page();
        }
    }
}
