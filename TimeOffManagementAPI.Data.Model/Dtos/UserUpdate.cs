using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserUpdate
{
    public string? Id { get; init; }

    public string? UserName { get; init; }

    [MaxLength(15)]
    public string? FirstName { get; init; }

    [MaxLength(15)]
    public string? LastName { get; init; }

    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Address { get; init; }

    public DateTime? DateOfBirth { get; init; }

    public DateTime? HireDate { get; init; }
}