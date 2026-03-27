using System.ComponentModel.DataAnnotations;

namespace KYPlayer.Models.ViewModels
{
    public class PlayerEditViewModel : PlayerCreateViewModel
    {
        public int PlayerId { get; set; }

        [Display(Name = "Current PSR")]
        public float CurrentPSR { get; set; }

        [Display(Name ="Total Ratings")]
        public int TotalRatingsCount { get; set; }

    }
}
