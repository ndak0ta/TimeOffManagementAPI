namespace TimeOffManagementAPI.Data.Model.Dtos;

public class UserInfo
{
    public string? Id { get; init; }
    public string? UserName { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public DateTime? HireDate { get; init; }
    public int AnnualTimeOffs { get; init; }
    public int RemainingAnnualTimeOffs { get; init; }
    public IList<string>? Roles { get; set; }

}