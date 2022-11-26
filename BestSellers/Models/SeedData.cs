// Have to 'Drop-Database' then 'Update-Database'
// every time changes are made in this file

using Microsoft.EntityFrameworkCore;
using BestSellers.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace BestSellers.Models
{
    public static class SeedData
    {
        // This model is just the necessary data from JSON
        // To get the entire JSON model do
        // 'Edit -> Paste Special -> Paste JSON as Classes'
        // when the whole JSON is in clipboard
        public class BestSeller
        {
            public int rank { get; set; }
            public string title { get; set; }
            public string author { get; set; }
        }

        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BestSellersContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BestSellersContext>>()))
            {
                if (context == null || context.Author == null || context.Book == null)
                {
                    throw new ArgumentNullException("Null BestSellersContext");
                }

                // DB has been seeded
                if (context.Author.Any())
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
                IList<BestSeller> bestSellerList = new List<BestSeller>();
                foreach (JToken result in results)
                {
                    BestSeller bestSeller = result.ToObject<BestSeller>();
                    bestSellerList.Add(bestSeller);
                }

                // Seed the database
                int i = 1;
                foreach (BestSeller bestSeller in bestSellerList)
                {
                    context.Author.AddRange(
                        new Author
                        {
                            Name = bestSeller.author
                        }
                    );
                    context.SaveChanges();
                    context.Book.AddRange(
                        new Book
                        {
                            Title = bestSeller.title,
                            Rank = bestSeller.rank,
                            AuthorID = i
                        }
                    );
                    context.SaveChanges();
                    i += 1;
                }
            }
        }
    }
}