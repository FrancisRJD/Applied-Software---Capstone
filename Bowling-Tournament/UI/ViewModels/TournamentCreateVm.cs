using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.UI.ViewModels
{
    public class TournamentCreateVm
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public DateTime DateOfGame { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Team capacity must be at least 1.")]
        public int TeamCapacity { get; set; }

        [Range(0, int.MaxValue)]
        public int WatcherCapacity { get; set; }

        public bool RegistrationOpen { get; set; }

        [Range(0, int.MaxValue)]
        public int MenCapacity { get; set; }

        [Range(0, int.MaxValue)]
        public int WomenCapacity { get; set; }

        [Range(0, int.MaxValue)]
        public int MixedCapacity { get; set; }

        [Range(0, int.MaxValue)]
        public int YouthCapacity { get; set; }

        [Range(0, int.MaxValue)]
        public int SeniorCapacity { get; set; }

    }
}