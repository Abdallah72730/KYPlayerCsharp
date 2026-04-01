using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace KYPlayer.Models
{
    public class Player
    {
        public int PlayerId { get; set; }

        [Required, StringLength(100)]
        public string PlayerName { get; set; }

        public int Age { get; set; }
        public int JerseyNumber { get; set; }
        public string? PhotoURL { get; set; }
        public float CurrentPSR { get; set; }
        public int TotalRatingsCount { get; set; }
        public string Position { get; set; }


        public PlayerSkills Skills { get; set; }
        public ICollection<Rating> Ratings { get; set; }

        public void UpdatePSR() 
        {
            if (Ratings != null && Ratings.Any())
            {
                CurrentPSR = (float)Ratings.Average(r => r.Value);
            }
            else 
            {
                CurrentPSR = 0.0f;
            }
        }



    }
}
