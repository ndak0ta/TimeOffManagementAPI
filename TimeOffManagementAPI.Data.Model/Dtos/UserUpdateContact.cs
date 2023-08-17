using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserUpdateContact
{
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }
}