using bowling_tournament_MVCPRoject.Domain.Entities;
using bowling_tournament_MVCPRoject.UI.ReadModels;

namespace bowling_tournament_MVCPRoject.UI.Queries
{
    public interface IRegistrationReadModelGateway
        //Gimme a quick heads up if you can if there's other read-only data you need from persistence! - Francis
    {
        Task<List<RegistrationListItem>> GetAllAsync();

        Task<List<RegistrationListItem>> GetAllPaidAsync();
    }
}
