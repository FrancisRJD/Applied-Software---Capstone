using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Daos
{
    public interface ITeamDao
    {
        /*
            Likely needed Dao methods
                READING team by teamID
                WRITING new team
                DELETING team by teamID
                    (You'd think that an update players or something might be needed by actually that's what the playerDao's for)
                Save changes
         */

        public TeamV2 findTeam(TeamV2 team); //Only uses teamId, other details ignored
        public void addTeam (TeamV2 team);
        public void editTeam (TeamV2 team);
        public void removeTeam(TeamV2 team); //Only uses teamId, other details ignored

        public void saveChanges();
    }
}
