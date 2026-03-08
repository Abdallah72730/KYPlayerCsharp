namespace KYPlayer.Models
{
    public class PlayerSkills
    {
        public int PlayerSkillsId { get; set; }
        public int PlayerId { get; set; }
        public int Shooting { get; set; }
        public int Passing { get; set; }
        public int Dribbling { get; set; }
        public int Vision { get; set; }
        public int Defense { get; set; }
        public int Finishing { get; set; }

        public float AverageSkillRating =>
            (Shooting + Passing + Dribbling + Vision + Defense + Finishing) / 6f;

        public Player Player { get; set; }
    }
}
