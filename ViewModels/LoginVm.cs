using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.ViewModels
{
    public class LoginVm
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
    }
}
