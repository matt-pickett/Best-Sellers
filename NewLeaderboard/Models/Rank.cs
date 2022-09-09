using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewLeaderboard.Models
{
    public class Rank
    {
        // Primary key. Can be defined as "ID" or "<classname>ID"
        // Global rank number. So rank 1, rank 2, etc.
        [Display(Name = "Rank")]
        public int RankID { get; set; }

        // Data should be: score or ELO rating
        public int Score { get; set; }

        // Foreign Key
        public int UserID { get; set; }
        // Reference navigation for foreign key - one to one relationship
        public User UserObj { get; set; }
    }
}
