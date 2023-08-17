using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class LoginResponse
{
    public string? JWT { get; set; }
    public UserInfo? UserInfo { get; set; }
}