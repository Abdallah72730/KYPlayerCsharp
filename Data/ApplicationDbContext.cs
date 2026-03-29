using KYPlayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KYPlayer.Areas.Identity.Data;

namespace KYPlayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerSkills> PlayerSkills { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            // One Player has one PlayerSkills (cascade: delete skills when player deleted)
            builder.Entity<Player>()
                .HasOne(p => p.Skills)
                .WithOne(s => s.Player)
                .HasForeignKey<PlayerSkills>(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // One Player can have many ratings (cascade: delete all ratings when player deleted)
            builder.Entity<Rating>()
                .HasOne(r => r.Player)
                .WithMany(p => p.Ratings)
                .HasForeignKey(r => r.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // One fan can have many ratings, but deleting a fan should not delete the ratings (restrict)
            builder.Entity<Rating>()
                .HasOne(r => r.Fan)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.FanId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
