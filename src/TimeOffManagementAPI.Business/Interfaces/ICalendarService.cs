namespace TimeOffManagementAPI.Business.Interfaces;

public interface ICalendarService
{
    List<DateTime> GetEidAlFitrDates();
    List<DateTime> GetEidAlAdhaDates();
    List<DateTime> GetPublicHolidays();
}