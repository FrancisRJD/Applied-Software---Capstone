using bowling_tournament_MVCPRoject.UI.ReadModels;
using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public interface IViewModelMapper
    {
        TeamEditVm MapTeamToEditVm(TeamListItem team, IEnumerable<SelectListItem> divisionOptions);
    }

    public class ViewModelMapper : IViewModelMapper
    {
        public TeamEditVm MapTeamToEditVm(TeamListItem team, IEnumerable<SelectListItem> divisionOptions)
        {
            return new TeamEditVm
            {
                TeamId = team.id,
                TeamName = team.teamName,
                DivisionId = team.teamDivision,
                DivisionOptions = divisionOptions,
                Players = team.Players
            };
        }
    }
}
