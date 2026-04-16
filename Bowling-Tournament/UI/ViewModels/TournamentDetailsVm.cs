using bowling_tournament_MVCPRoject.UI.ReadModels;

namespace bowling_tournament_MVCPRoject.UI.ViewModels
{
    public class TournamentDetailsVm
    {
        public TournamentListItem? Tournament { get; set; }
        public List<RegistrationListItem>? Registrations { get; set; }
    }
}
