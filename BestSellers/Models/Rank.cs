﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BestSellers.Models
{
    public class Book
    {
        // Primary key. Can be defined as "ID" or "<classname>ID"
        // Global rank number. So rank 1, rank 2, etc.
        public int BookID { get; set; }

        // Data should be: score or ELO rating
        public string Title { get; set; }

        public int Rank { get; set; }

        // Foreign Key
        public int AuthorID { get; set; }
        // Reference navigation for foreign key - one to one relationship
        public Author AuthorObj { get; set; }
    }
}
