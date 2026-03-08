namespace KYPlayer.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public float AverageSeasonPSR { get; set; }

        //Navigation Property
        public Player Player { get; set; }

        public void AnalyzeTrends() { }
    }
}
