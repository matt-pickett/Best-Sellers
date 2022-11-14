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

        public DbSet<Author> Author { get; set; } = default!;
        public DbSet<Book> Book { get; set; } = default!;
    }
}
