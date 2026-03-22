using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.Domain.Entities
{
    //New Player entity set up to work with the re-made Team class(TeamV2). Otherwise identical to Player
    //  minus the data annotations

    public class PlayerV2
    {

        public int PlayerId { get; set; }

        public int TeamId { get; set; }
        public TeamV2 Team { get; set; }

        public string PlayerName { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
