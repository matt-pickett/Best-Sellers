using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewLeaderboard.Models;

namespace NewLeaderboard.Data
{
    public class NewLeaderboardContext : DbContext
    {
        public NewLeaderboardContext (DbContextOptions<NewLeaderboardContext> options)
            : base(options)
        {
        }

        public DbSet<NewLeaderboard.Models.User> User { get; set; } = default!;
        public DbSet<NewLeaderboard.Models.Rank> Rank { get; set; } = default!;
    }
}
