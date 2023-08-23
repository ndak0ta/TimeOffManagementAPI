using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserLogin
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(255)]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Password is required")]
    [MaxLength(255)]
    public string? Password { get; init; }
}
