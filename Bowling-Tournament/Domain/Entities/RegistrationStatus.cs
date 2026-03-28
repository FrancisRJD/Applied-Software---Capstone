namespace bowling_tournament_MVCPRoject.Domain.Entities
{
    public enum RegistrationStatus 
        //I've decided to make RegistrationStatus into an enum instead of a bool incase status needs to be expanded on!
    {
        Unpaid = 0,
        Paid = 1,   
        OnHold = 2,
        Registered = 3
    }
}
