using System.ComponentModel.DataAnnotations;

namespace KYPlayer.Models.ViewModels
{
    public class PlayerCreateViewModel
    {
        [Required(ErrorMessage = "Player name is required")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string PlayerName { get; set; }


        [Required]
        [Range(15, 45, ErrorMessage = "Age must be between 15 and 45")]
        public int Age { get; set; }

        [Required]
        [Range(1, 99)]
        [Display(Name = "Jersey Number")]
        public int JerseyNumber { get; set; }

        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Photo URL")]
        [Url(ErrorMessage ="Please enter a valid URL")]
        public string? PhotoURL { get; set; }

        [Range(1, 100)] public int Shooting { get; set; } = 50;
        [Range(1, 100)] public int Passing { get; set; } = 50;
        [Range(1, 100)] public int Dribbling { get; set; } = 50;
        [Range(1, 100)] public int Vision { get; set; } = 50;
        [Range(1, 100)] public int Defense { get; set; } = 50;
        [Range(1, 100)] public int Finishing { get; set; } = 50;
    }
}
