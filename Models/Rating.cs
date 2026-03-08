using System.ComponentModel.DataAnnotations;

namespace KYPlayer.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string FanId { get; set; }
        public int PlayerId { get; set; }

        //Navigation
        public ApplicationUser Fan { get; set; }
        public Player Player { get; set; }
    }
}
