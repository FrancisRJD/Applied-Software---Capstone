using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Dtos.Results
{
    public class TeamResult
    {

        public bool success { get; set; } = false;
        public List<string> Errors { get; set; } = new List<string>();

        public TeamV2 team { get; set; } = new TeamV2(); //Theoretically reusing the team class should be fine for returns...
            //Added a team return as the service *may* need to return a specific team and its details
    }
}
