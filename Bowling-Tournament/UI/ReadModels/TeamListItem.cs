namespace bowling_tournament_MVCPRoject.UI.ReadModels
{
    public class TeamListItem
    {
        public int id {  get; set; }
        public string? teamName { get; set; }
        public int teamDivision { get; set; }
        public List<PlayerListItem> Players { get; set; } = new();
    }
}
