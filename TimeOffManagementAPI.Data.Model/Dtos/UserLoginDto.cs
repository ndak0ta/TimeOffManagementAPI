using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserLoginDto
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(255)]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MaxLength(255)]
    public string? Password { get; set; }
}
