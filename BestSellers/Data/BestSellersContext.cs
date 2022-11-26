using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BestSellers.Models;

namespace BestSellers.Data
{
    public class BestSellersContext : DbContext
    {
        public BestSellersContext (DbContextOptions<BestSellersContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Author { get; set; } = default!;
        public DbSet<Book> Book { get; set; } = default!;
    }
}
