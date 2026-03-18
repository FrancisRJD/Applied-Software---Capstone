# Project's .gitignore should now function THIS TIME and not be a complete mess! If uneditable files that shouldn't be pushed up to the repo are getting pushed, give me (Francis) a heads up so I can make sure it's ripped out of the repo and added to the .gitignore list!

## TODO
### OVERARCHING
- Refactor project so it follows current architectural rules (Everything resides in view, everything is tightly-coupled)
- Add new features not currently included in the OG project
- Determine whether user authorization should be verified in View or strictly in Domain
	- Should User.HasClaim() and related calls be moved to Domain?

### VIEW
- Implement Tournament creation, editting, and view (ADMIN ONLY)
	- Tournament List
	- Create Tournament Form
	- Edit Tournament Form
- Separate Team list view into a team registration view and a team view
	- Registration view is mainly for admins to see new registrations, mark payments, change status, etc
		- Displays from (To-be-added) TeamRegistration table
		- (Non-critical) Probably should also display team details instead of just team ID (For clarity)
	- Teams view is mainly public-facing.
		- Non-admin view should display teams that have PAID status on database
		- Admin view should display all teams and allow for editting (Admin presumed to be the ultimate arbiter of editting/removing stuff basically)
- Alter Team List view to only show teams registered to the current selected tournament
- Refactor controllers to be more reliant on result returns from domain for error display (Where needed)
- (Large chunk of the view has already been done by the MVC project so the grand bulk of the View work is in implementing the new views/altering existing views to account for the fact that we're dealing with *multiple* tournaments now)

### DOMAIN
- Move entities out of Models to make them the Domain's responsibility.
- Refactor View controllers to decouple persistence and view, and move business rule checks to itself.
	- Admin Controller
		- Index
		- CreateTeam (GET/POST)
		- EditTeam (GET/POST)
		- DeleteTeam (GET)
		- DeleteTeamConfirmed (POST)
		- UpdatePlayer (POST) - How is this getting called? Seems to be getting editted within Admin>Edit
		- AddPlayer(GET/POST) - Not actually being used? (Instead using wwwroot javascript magic)
		- RemovePlayer (POST)
		- MarkPaid
		- Summary
		- TeamListAdmin
		- DetailsAdmin
	- Home Controller
		- TeamList
		- Details
		- Register (GET/POST)
		- RegisterPlayer - Not actually being used? (Probably same as admin variant?)
- Slated for possible removal (To avoid confusion)
	- Admin Controller
		- > AddPlayer (Unless it's being used by js?)
	- Home Controller
		- > RegisterPlayer (Unless it's being used by js?)
- Auth Controller Refactor (Separate from above controller refactors as this is non-critical, but needed eventually)

### PERSISTENCE
- Update database (And especially the rebuild DB SQL file!) to follow structure
	- Player
		- Id - Int
		- TeamId - Int, Foreign Key (Team)
		- Name - String
		- Email - String
		- City - String 
		- Province - String
	- Team
		- Id - Int
		- Name - String
		- DivisionId - Int
		- TournamentId - Int, Foreign Key (Tournament)
	- Tournament
		- Id - Int
		- Name - String
		- TournamentDate - DateTime
		- Location - String
		- TeamCapacity - Int
		- RegistrationOpen - Bool
		- TournamentCapcity - Int (For viewer capacity?)
	- TournamentRegistration
		- Id - Int
		- TournamentId - Int, Foreign Key (Tournament)
		- TeamId - Int, Foreign Key (Team)
		- Status - ENUM RANGE (0 = Unpaid, 1 = Paid) (For verifying if team has paid or not, for now)
		- StatusDate - DateTime (Time of status change)
	- User (Non-critical, but needed)
		- Id - Int
		- UserName - String
		- PasswordHash - String
		- IsAdmin - Bool
- Move DbContext out of the Models folder
- Add Dao(s) for Domain to request read-write from


## Git Cmd Notes To Self
git status - Shows what files are being added, removed, or modified. Doublecheck using this to ensure "Bin", "Obj", "Log", "Build" and other generated non-critical files/folders aren't getting pushed up to the repo!!!