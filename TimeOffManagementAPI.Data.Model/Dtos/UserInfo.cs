using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserInfo
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateTime? BirthDate { get; init; }
    public DateTime? HireDate { get; init; }
    public string? Address { get; init; }
    public int AnnualTimeOffs { get; init; }
    public int RemainingAnnualTimeOffs { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }

}