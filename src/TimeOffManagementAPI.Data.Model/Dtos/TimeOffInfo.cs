namespace TimeOffManagementAPI.Data.Model.Dtos;

public class TimeOffInfo
{
    public int Id { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int TotalDays { get; init; }
    public string? Status { get; init; }
    public DateTime? CreatedAt { get; init; }
    public string? UserId { get; init; }
}