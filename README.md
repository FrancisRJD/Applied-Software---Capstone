# NBCC Bowling League - Application Documentation

## Overview

**Created by:** Nicholas Perry, Matthew Pyne, Francis Roney

---

## Application Purpose

The NBCC Bowling League application serves three primary functions:

### 1. **Team Management**
- Create and manage bowling teams
- Add players to teams (4 players per team)
- Edit team details and player information
- Track team payment status

### 2. **Tournament Organization**
- Create and schedule tournaments
- Set team and spectator capacity limits
- Configure division-specific capacity limits
- Enable/disable team registration periods

### 3. **Registration & Waitlisting**
- Register teams for tournaments
- Automatically manage waitlists based on capacity
- Track payment status
- Process registration payments

---

## Getting Started

### First Time Access

1. **Navigate to the Application**
   - Open your web browser and go to the application URL
   - You'll see the NBCC Bowling League home page

2. **Main Navigation Menu**
   - **Home** - Returns to the home page
   - **Team List** - View all registered teams
   - **Tournament List** - View upcoming tournaments
   - **Register Team** - Create a new team
   - **Admin Login** - Access admin features (top right)

---

## User Guide

#### Viewing Teams

1. Navigate to **Team List**
2. Teams are organized by division:
   - Men's Division
   - Women's Division
   - Mixed Division
   - Youth Division
   - Senior Division
3. Each team displays:
   - Team name
   - Payment status (Paid/Unpaid)
   - Team details link

#### Viewing Team Details

1. Click the **Details** button on any team
2. View team information including:
   - Team name and division
   - Payment status
   - List of 4 team players with contact information

#### Viewing Tournaments

1. Navigate to **Tournament List**
2. Tournaments are organized by date (soonest first)
3. For each tournament, see:
   - Tournament name and location
   - Date and time
   - Division capacity information
   - Overall team capacity remaining
4. Click **View** to see tournament details including registered teams

#### Registering a Team

1. Click **Register Team** in the navigation menu
2. Fill in the team information:
   - **Team Name** - Name of your bowling team
   - **Division** - Select your team's division from the dropdown
3. Add 4 players:
   - **Player Name** (required)
   - **Email** (required)
   - **City** (required)
   - **Province** (required)
   - **Phone** (required)
4. Click **Register Team**
5. **Important:** Your registration is NOT complete until payment is received
   - An admin must mark the team as paid
   - Only paid teams can register for tournaments

#### Viewing Tournament Details

1. From **Tournament List**, click **View** on any tournament
2. See:
   - Full tournament information
   - Registered teams and their status
   - Waitlisted teams (if applicable)
   - Division capacity information

---

### Admin Login

1. Click **Admin Login** (top right of navigation)
2. Enter your credentials:
   - smonk
   - password
3. Click **Sign In**

---

## Tournament Management

### Creating a Tournament

1. Click **Create Tournament** in the navigation menu
2. Fill in tournament details:

**Basic Information:**
- **Tournament Name** - e.g., "Spring 2025 Championship"
- **Tournament Date** - Select from calendar
- **Location** - e.g., "Downtown Bowling Alley"

**Capacity Settings:**
- **Max Teams** - Overall team limit for the tournament
- **Spectator Capacity** - Audience capacity (if applicable)

**Division Capacities:**
- **Men's Division** - Enter capacity or -1 for unlimited
- **Women's Division** - Enter capacity or -1 for unlimited
- **Mixed Division** - Enter capacity or -1 for unlimited
- **Youth Division** - Enter capacity or -1 for unlimited
- **Senior Division** - Enter capacity or -1 for unlimited

**Registration:**
- **Allow Team Registration** - Check to enable team signups

3. Click **Create Tournament**

### Editing a Tournament

1. From **Tournament List**, click **Edit** on the tournament
2. Modify any tournament settings
3. Click **Save Changes**

### Deleting a Tournament

1. From **Tournament List**, click **Delete** on the tournament
2. Review the tournament details
3. Click **Confirm Deletion** (this cannot be undone)
4. You'll be redirected to the tournament list

---

## Payment & Registration

### Admin: Team Management

#### View All Teams

1. Teams page shows all registered teams organized by division
2. Each team displays:
   - Team name
   - Payment status (Paid/Unpaid badge)
   - Action buttons

#### Mark Team as Paid

1. From the **Team List**, find the team
2. Click **Mark Paid** button
3. Team payment status updates immediately
4. Team is now eligible to register for tournaments

#### Edit Team Details

1. Click **Edit** on the team
2. Modify:
   - Team name
   - Division
3. Click **Save Changes**

#### Edit Team Players

1. Click **Edit** on the team
2. Click **Edit** or **Remove** on individual players
3. Add new players by clicking **Add Player**
4. Fill in player details (all fields required)
5. Click **Save**

#### Delete a Team

1. Click **Delete** on the team
2. Review team information
3. Click **Confirm Deletion** (this cannot be undone)

### Admin: Team Registration

#### Register Team for Tournament

1. Click **Register** on a team
2. Select the tournament from the dropdown
3. Click **Register**
4. Team is automatically:
   - **Registered** if space available in division and overall capacity
   - **Waitlisted** if at capacity but interested

**Note:** Only paid teams can register for tournaments

#### View Tournament Registrations

1. From **Tournament List**, click **View** on a tournament
2. See all registered and waitlisted teams
3. Click **View Waitlist** to see waitlisted teams
4. If a registered team cancels, the first waitlisted team from the same division is automatically promoted

#### Cancel Team Registration

1. From **Tournament Details**, click **Cancel** on a registered team
2. Team is removed from the tournament
3. If from same division, the first waitlisted team is automatically promoted to registered status

### Payment Tracking

#### View Payment Summary

1. From the navigation menu, click **Team List**
2. See payment overview table showing:
   - Division names
   - Total teams per division
   - Paid teams per division
   - Total fees collected
3. Three summary cards display:
   - **Total Teams** - All teams registered
   - **Paid Teams** - Teams that have paid with completion percentage
   - **Total Revenue** - Total fees collected at $200 per team
