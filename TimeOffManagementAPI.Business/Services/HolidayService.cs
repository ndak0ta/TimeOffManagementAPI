using System.Text.Json;
using System.Text.Json.Serialization;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Services;

public class HolidayService : IHolidayService
{
    private readonly IHolidayRepository _holidayRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public HolidayService(IHolidayRepository holidayRepository, IHttpClientFactory httpClientFactory)
    {
        _holidayRepository = holidayRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<Holiday>> GetAllAsync()
    {
        return await _holidayRepository.GetAllAsync();
    }

    public async Task<Holiday> GetByIdAsync(int id)
    {
        return await _holidayRepository.GetByIdAsync(id);
    }

    public async Task<Holiday> CreateAsync(Holiday holiday)
    {
        return await _holidayRepository.CreateAsync(holiday);
    }

    public async Task<Holiday> UpdateAsync(Holiday holiday)
    {
        return await _holidayRepository.UpdateAsync(holiday);
    }

    public async Task DeleteAsync(int id)
    {
        await _holidayRepository.DeleteAsync(id);
    }

    public async Task UpdateHolidaysFromAPI()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync("https://api.ubilisim.com/resmitatiller/");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response, options);

        var holidays = apiResponse?.Holidays;

        if (holidays == null)
            throw new Exception("Holidays could not be retrieved from API");

        foreach (var holiday in holidays)
        {
            var existingHoliday = await _holidayRepository.GetByDate(holiday.Date);

            if (existingHoliday == Enumerable.Empty<Holiday>())
            {
                await _holidayRepository.CreateAsync(holiday);
            }
        }
    }
}

public class ApiResponse // TODO daha sonra düzelt
{
    public bool Success { get; set; }
    public string? Status { get; set; }
    public string? PageCreateDate { get; set; }

    [JsonPropertyName("resmitatiller")]
    public List<Holiday>? Holidays { get; set; }
}