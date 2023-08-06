namespace TimeOffManagementAPI.Data.Model.Dtos;

public class TimeOffCancelRequest
{
    public int TimeOffId { get; set; }
    public string? UserId { get; set; }
}