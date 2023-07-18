using Microsoft.AspNetCore.Identity;

namespace TimeOffManagementAPI.Data.Model.Models;

public class Role : IdentityRole
{
    public Role()
    {
        IsActive = true;
    }

    public bool IsActive { get; set; }
}
