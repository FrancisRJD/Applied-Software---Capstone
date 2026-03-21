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

        public Team findTeam(Team team); //Only uses teamId, other details ignored
        public void addTeam (Team team);
        public void removeTeam(Team team); //Only uses teamId, other details ignored

        public void saveChanges();
    }
}
