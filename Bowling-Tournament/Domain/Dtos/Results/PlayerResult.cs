namespace bowling_tournament_MVCPRoject.Domain.Dtos.Results
{
    public class PlayerResult
    {
        public bool success {  get; set; } = false;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
