using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserRegistration
{
    [Required(ErrorMessage = "Username is required")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; init; }

    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; init; }

    [MaxLength(15)]
    [Required(ErrorMessage = "First name is required")]
    public string? FirstName { get; init; }

    [MaxLength(15)]
    [Required(ErrorMessage = "Last name is required")]
    public string? LastName { get; init; }

    public DateTime? DateOfBirth { get; init; }

    [Required(ErrorMessage = "Employment date is required")]
    public DateTime? HireDate { get; init; }

    public string? PhoneNumber { get; init; }
}