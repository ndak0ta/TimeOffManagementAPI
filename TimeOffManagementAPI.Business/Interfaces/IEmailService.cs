namespace TimeOffManagementAPI.Business.Interfaces;

public interface IEmailService
{
    public Task SendEmaiAsync(string recipient, string subject, string body);
}
