using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BestSellers.Data;
using BestSellers.Models;

namespace BestSellers.Pages.Leaderboard
{
    public class IndexModel : PageModel
    {
        private readonly BestSellers.Data.BestSellersContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(BestSellers.Data.BestSellersContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // Values get queried in the view
        public string TitleSort { get; set; }
        public string RankSort { get; set; }
        public string CurrentSearch{ get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Author> AuthorObj { get; set; } = default!;


        // These args are set when they get referenced by "asp-route" in the view
        // or as part of the URL when they get referenced in HTML page
        public async Task OnGetAsync(string sortOrder, string searchString, string currentSearch, int? pageIndex)
        {

            IQueryable<Author> leaderboardOps = from s in _context.Author
                                              select s;

            // '? :' is conditional ternary operator which evaluates to true or false (just an if else statement)
            // All it really does is make it alternate sorting by ascending and descending,
            // starting with ascending
            TitleSort = sortOrder == "title" ? "title_desc" : "title";
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
                leaderboardOps = leaderboardOps.Where(author => author.Book.Title.Contains(searchString) || (author.Book.Rank).ToString() == (searchString));
            }

            // Change sort value
            CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "title_desc":
                    leaderboardOps = leaderboardOps.OrderByDescending(author => author.Book.Title);
                    break;
                case "title":
                    leaderboardOps = leaderboardOps.OrderBy(author  => author.Book.Title);
                    break;
                case "rank_desc":
                    leaderboardOps = leaderboardOps.OrderByDescending(author => author.Book.Rank);
                    break;
                default:
                    leaderboardOps = leaderboardOps.OrderBy(author => author.Book.Rank);
                    break;
            }
            
            // Get "Page Size" value from appsettings.json, set it to 5 if can't be found
            var pageSize = Configuration.GetValue("PageSize", 5);

            AuthorObj = await PaginatedList<Author>
                .CreateAsync(
                    leaderboardOps
                    // Also initialize Rank as part of User (it has foreign key to User)
                    // it can be called by 'userObj.Rank'
                    .Include(author => author.Book)
                    .AsNoTracking(), 
                    // If pageIndex exists, set it to that, otherwise 1
                    pageIndex ?? 1, 
                    pageSize);
        }
    }
}
