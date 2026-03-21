using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
using bowling_tournament_MVCPRoject.Domain.Dtos.Results;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public interface ITournamentService
    {
        TournamentResult tryRegisterTournament (TournamentRequest request);
            //Request contains tournament details minus tournamentID.
        TournamentResult tryUpdateTournament (TournamentRequest request);
            //Request contains tournament details plus tournamentID.

        //I'd add a try tournament delete but that's not strictly necessary yet so this'll do.
    }
}
