// Have to 'Drop-Database' then 'Update-Database'
// every time changes are made in this file

using Microsoft.EntityFrameworkCore;
using NewLeaderboard.Data;

namespace NewLeaderboard.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new NewLeaderboardContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<NewLeaderboardContext>>()))
            {
                if (context == null || context.User == null || context.Rank == null)
                {
                    throw new ArgumentNullException("Null NewLeaderboardContext");
                }

                // DB has been seeded
                if (context.User.Any())
                {
                    return;   
                }

                context.User.AddRange(
                    new User
                    {
                        Name = "John Smith"
                    },

                    new User
                    {
                        Name = "Elly Mckenzie"
                    },

                    new User
                    {
                        Name = "Nino Olivetto"
                    },

                    new User
                    {
                        Name = "Laura Norman"
                    },

                    new User
                    {
                        Name = "Yan Li"
                    }
                );
                context.SaveChanges();
                context.Rank.AddRange(
                    new Rank
                    {
                        Score = 100,
                        UserID = 4
                    },

                    new Rank
                    {
                        Score = 98,
                        UserID = 3
                    },

                    new Rank
                    {
                        Score = 87,
                        UserID = 1
                    },

                    new Rank
                    {
                        Score = 82,
                        UserID = 2
                    },

                    new Rank
                    {
                        Score = 75,
                        UserID = 5
                    }
                );
                context.SaveChanges();
            }
        }
    }
}