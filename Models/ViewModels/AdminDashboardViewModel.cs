namespace KYPlayer.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalPlayers { get; set; }
        public int TotalFans { get; set; }
        public int TotalRatings { get; set; }
        public Player? TopPlayer { get; set; }
        public List<Rating> RecentRatings { get; set; } = new();

    }
}
