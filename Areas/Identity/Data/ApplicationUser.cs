using KYPlayer.Models;
using Microsoft.AspNetCore.Identity;
namespace KYPlayer.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    { 
        public string Name { get; set; }
        public string CollegeEmail { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}
