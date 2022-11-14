using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewLeaderboard.Models
{
    public class Author
    {
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        // Primary key. Can be defined as "ID" or "<classname>ID"
        public int AuthorID { get; set; }

        [DisplayFormat(NullDisplayText = "No name")]
        [Display(Name = "Author")]
        // Data should be: user's actual name, such as "John Smith"
        public string Name { get; set; }

        // Reference navigation property so that EFCore knows we have a foreign key in Rank
        // One to one relationship
        public Book Book { get; set; }

        // ICollection like this is used to represent a many relationship
        //public ICollection<Rank> Rank { get; set; }
    }
}

