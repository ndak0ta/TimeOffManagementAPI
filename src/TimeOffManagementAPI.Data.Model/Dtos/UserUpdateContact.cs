using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserUpdateContact
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; init; }

    [MaxLength(19)]
    public string? PhoneNumber { get; init; }
}