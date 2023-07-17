using Microsoft.AspNetCore.Identity;

namespace TimeOffManagementAPI.Data.Model.Models;

public class User : IdentityUser
{
    public User()
    {
        AnnualTimeOffs = 14;
        isActive = true;
    }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime HireDate { get; set; }

    public int AnnualTimeOffs { get; set; }

    public bool isActive { get; set; }
}