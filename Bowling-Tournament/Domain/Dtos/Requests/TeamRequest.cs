namespace bowling_tournament_MVCPRoject.Domain.Dtos.Requests
{
    public class TeamRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DivisionId { get; set; }

        public TeamRequest(int id, string name, int division)
        {
            this.Id = id;
            this.Name = name;
            this.DivisionId = division;
        }
    }
}
