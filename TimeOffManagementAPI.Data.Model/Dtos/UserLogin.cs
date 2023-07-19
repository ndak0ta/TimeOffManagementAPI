using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserLogin
{
    public UserLogin()
    {
        RememberMe = false;
    }

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(255)]
    public string? Username { get; init; }

    [Required(ErrorMessage = "Password is required")]
    [MaxLength(255)]
    public string? Password { get; init; }

    public bool RememberMe { get; init; }
}
