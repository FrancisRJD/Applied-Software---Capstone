using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class TeamCreateVm
{
    [Required]
    public string TeamName { get; set; }

    [Required]
    public int DivisionId { get; set; }

    public List<PlayerEditVm> NewPlayers { get; set; } = new();

    [ValidateNever]
    public IEnumerable<SelectListItem> DivisionOptions { get; set; }
}
