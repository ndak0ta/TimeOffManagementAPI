using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserRegistration
{
    public UserRegistration()
    {
        HireDate = DateTime.Now;
    }

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

    [Required(ErrorMessage = "Birth date is required")]
    public DateTime? DateOfBirth { get; init; }

    public DateTime? HireDate { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Address { get; init; }
}