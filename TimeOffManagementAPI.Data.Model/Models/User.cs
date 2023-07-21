using Microsoft.AspNetCore.Identity;

namespace TimeOffManagementAPI.Data.Model.Models;

public class User : IdentityUser
{
    public User()
    {
        AnnualTimeOffs = 0;
        RemainingAnnualTimeOffs = AnnualTimeOffs;
        isActive = true;
        LockoutEnd = DateTime.Now + TimeSpan.FromMinutes(5); // TODO sonra revize et
        AutomaticAnnualTimeOffIncrement = true;
    }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime HireDate { get; set; }

    public int AnnualTimeOffs { get; set; }

    public int RemainingAnnualTimeOffs { get; set; }

    public bool AutomaticAnnualTimeOffIncrement { get; set; }

    public bool isActive { get; set; }

    public ICollection<TimeOff>? TimeOffs { get; set; }
}