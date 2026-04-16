namespace bowling_tournament_MVCPRoject.UI.ReadModels
{
    public class TournamentCapacityInfo
    {
        public int MenRemaining { get; set; }
        public int WomenRemaining { get; set; }
        public int MixedRemaining { get; set; }
        public int YouthRemaining { get; set; }
        public int SeniorRemaining { get; set; }

        public int MenLimit { get; set; }
        public int WomenLimit { get; set; }
        public int MixedLimit { get; set; }
        public int YouthLimit { get; set; }
        public int SeniorLimit { get; set; }
    }
}
