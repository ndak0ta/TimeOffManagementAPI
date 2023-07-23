using System.Globalization;
using TimeOffManagementAPI.Business.Interfaces;


namespace TimeOffManagementAPI.Business.Services;

public class CalendarService : ICalendarService
{
    public List<DateTime> GetEidAlFitrDates()
    {
        int year = DateTime.UtcNow.Year;
        var islamicCalendar = new HijriCalendar();
        int month = islamicCalendar.GetMonth(new DateTime(year, 1, 1));

        List<DateTime> ramadanDates = new List<DateTime>();

        while (month != 9)
        {
            year++;
            month = islamicCalendar.GetMonth(new DateTime(year, 1, 1));
        }

        int day = 1;
        for (int i = 0; i < 3; i++)
        {
            while (month == 9)
            {
                day++;
                month = islamicCalendar.GetMonth(new DateTime(year, 1, day));
            }

            ramadanDates.Add(new DateTime(year, 1, day - 1));
            day++;
            month = islamicCalendar.GetMonth(new DateTime(year, 1, day));
        }

        return ramadanDates;
    }

    public List<DateTime> GetEidAlAdhaDates()
    {
        int year = DateTime.UtcNow.Year;
        var islamicCalendar = new HijriCalendar();
        int month = islamicCalendar.GetMonth(new DateTime(year, 12, 1));

        List<DateTime> eidAlAdhaDates = new List<DateTime>();

        while (month != 12)
        {
            year++;
            month = islamicCalendar.GetMonth(new DateTime(year, 12, 1));
        }

        int day = 1;
        for (int i = 0; i < 3; i++)
        {
            while (month == 12)
            {
                day++;
                month = islamicCalendar.GetMonth(new DateTime(year, 12, day));
            }

            eidAlAdhaDates.Add(new DateTime(year, 12, day - 1));
            day++;
            month = islamicCalendar.GetMonth(new DateTime(year, 12, day));
        }

        return eidAlAdhaDates;
    }

    public List<DateTime> GetPublicHolidays()
    {
        List<DateTime> publicHolidays = new List<DateTime>();

        publicHolidays.Add(new DateTime(DateTime.UtcNow.Year, 1, 1));
        publicHolidays.Add(new DateTime(DateTime.UtcNow.Year, 5, 1));

        return publicHolidays;
    }
}