using bowling_tournament_MVCPRoject.UI.ReadModels;
using bowling_tournament_MVCPRoject.UI.ViewModels;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public interface ISummaryService
    {
        List<SummaryVM> CalculateSummary(List<TeamListItem> teams);
    }

    public class SummaryService : ISummaryService
    {
        public List<SummaryVM> CalculateSummary(List<TeamListItem> teams)
        {
            var summary = teams
                .GroupBy(t => t.divisionName ?? "Unknown")
                .Select(g => new SummaryVM
                {
                    DivisionName = g.Key,
                    Teams = g.Count(),
                    PayingTeams = g.Count(t => t.isPaid),
                    TotalFees = g.Count(t => t.isPaid) * AppConstants.TEAM_REGISTRATION_FEE
                }).ToList();

            summary.Add(new SummaryVM
            {
                DivisionName = "Overall",
                Teams = teams.Count,
                PayingTeams = teams.Count(t => t.isPaid),
                TotalFees = teams.Count(t => t.isPaid) * AppConstants.TEAM_REGISTRATION_FEE
            });

            return summary;
        }
    }
}
