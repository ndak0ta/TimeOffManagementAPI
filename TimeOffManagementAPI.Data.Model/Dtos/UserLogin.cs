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
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MaxLength(255)]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}
