// Have to 'Drop-Database' then 'Update-Database'
// every time changes are made in this file

using Microsoft.EntityFrameworkCore;
using NewLeaderboard.Data;

using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace NewLeaderboard.Models
{
    public static class SeedData
    {
        // This model is just the necessary data from JSON
        // To get the entire JSON model do
        // 'Edit -> Paste Special -> Paste JSON as Classes'
        // when the whole JSON is in clipboard
        public class Book
        {
            public int rank { get; set; }
            public string title { get; set; }
            public string author { get; set; }
        }

        /*
        public class Todos
        {
            public int userId { get; set; }
            public int id { get; set; }
            public string title { get; set; }
            public bool completed { get; set; }
        }
        */

        public static async void Initialize(IServiceProvider serviceProvider)
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

                // Make a call to web API
                HttpClient client = new HttpClient();
                var json = await client.GetStringAsync(
                "https://api.nytimes.com/svc/books/v3/lists/current/hardcover-fiction.json?api-key=2OT9MLIjIJiiPJTIhSFSsorRYQDG6DE3");
                
                // Parse JSON
                JObject root = JObject.Parse(json);
                IList<JToken> results = root["results"]["books"].Children().ToList();
                IList<Book> bookList = new List<Book>();
                foreach (JToken result in results)
                {
                    Book book = result.ToObject<Book>();
                    bookList.Add(book);
                }

                // Seed the database
                int i = 1;
                foreach (Book book in bookList)
                {
                    // Turn identity insert on so that Rank model's primary key
                    // can be manually set
                    // It needs to be wrapped in this transaction to work
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.User.AddRange(
                            new User
                            {
                                Name = book.author
                            }
                        );
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Rank ON;");
                        context.Rank.AddRange(
                            new Rank
                            {
                                RankID = book.rank,
                                Title = book.title,
                                UserID = i
                            }
                        );
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Rank OFF;");
                        transaction.Commit();
                    }
                    i += 1;
                }

                /*context.User.AddRange(
                       new User
                       {
                           Name = book.author
                       }
                   );
                   //context.SaveChanges();

                   context.Rank.AddRange(
                           new Rank
                           {
                               Title = book.title,
                               UserID = i
                           }
                       );
                   context.SaveChanges();
                   i += 1;
                   */

                /*
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
                */
            }
        }
    }
}