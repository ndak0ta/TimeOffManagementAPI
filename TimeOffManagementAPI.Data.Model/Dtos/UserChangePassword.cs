using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserChangePassword
{
    public string? Id { get; init; }

    [Required(ErrorMessage = "Old password is required")]
    public string? OldPassword { get; init; }

    [Required(ErrorMessage = "New password is required")]
    public string? NewPassword { get; init; }
}