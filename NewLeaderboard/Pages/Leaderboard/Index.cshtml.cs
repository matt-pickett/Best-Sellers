using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewLeaderboard.Data;
using NewLeaderboard.Models;

namespace NewLeaderboard.Pages.Leaderboard
{
    public class IndexModel : PageModel
    {
        private readonly NewLeaderboard.Data.NewLeaderboardContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(NewLeaderboard.Data.NewLeaderboardContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // Values get queried in the view
        public string NameSort { get; set; }
        public string RankSort { get; set; }
        public string CurrentSearch{ get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<User> UserObj { get; set; } = default!;


        // These args are set when they get referenced by "asp-route" in the view
        // or as part of the URL when they get referenced in HTML page
        public async Task OnGetAsync(string sortOrder, string searchString, string currentSearch, int? pageIndex)
        {

            IQueryable<User> leaderboardOps = from s in _context.User
                                              select s;

            // '? :' is conditional ternary operator which evaluates to true or false (just an if else statement)
            // All it really does is make it alternate sorting by ascending and descending,
            // starting with ascending
            NameSort = sortOrder == "name" ? "name_desc" : "name";
            RankSort = String.IsNullOrEmpty(sortOrder) ? "rank_desc" : "";

            // Paging
            // If we are searching, just display one result
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentSearch;
            }

            // Searching
            CurrentSearch = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                leaderboardOps = leaderboardOps.Where(user => user.Name.Contains(searchString) || (user.Rank.RankID).ToString() == (searchString));
            }

            // Change sort value
            switch (sortOrder)
            {
                case "name_desc":
                    leaderboardOps = leaderboardOps.OrderByDescending(user => user.Name);
                    break;
                case "name":
                    leaderboardOps = leaderboardOps.OrderBy(user  => user.Name);
                    break;
                case "rank_desc":
                    leaderboardOps = leaderboardOps.OrderByDescending(user => user.Rank.RankID);
                    break;
                // Rank ascending is the default sorting value
                default:
                    leaderboardOps = leaderboardOps.OrderBy(user => user.Rank.RankID);
                    break;
            }
            // Get "Page Size" value from appsettings.json, set it to 5 if can't be found
            var pageSize = Configuration.GetValue("PageSize", 5);

            UserObj = await PaginatedList<User>
                .CreateAsync(
                    leaderboardOps
                    // Also initialize Rank as part of User (it has foreign key to User)
                    // it can be called by 'userObj.Rank'
                    .Include(user => user.Rank)
                    .AsNoTracking(), 
                    // If pageIndex exists, set it to that, otherwise 1
                    pageIndex ?? 1, 
                    pageSize);
        }
    }
}
