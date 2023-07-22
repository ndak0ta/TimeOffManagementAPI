using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace TimeOffManagementAPI.Data.Model.Models;


public class Holiday // TODO date işlemleri için dto oluştur
{
    [NotMapped]
    private DateTime _date;

    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    [JsonPropertyName("gun")]
    public string? HolidayNameTr { get; set; }

    [MaxLength(50)]
    [JsonPropertyName("en")]
    public string? HolidayNameEn { get; set; }

    [MaxLength(50)]
    [JsonPropertyName("haftagunu")]
    public string? DayOfWeekTr { get; set; }

    [NotMapped]
    [JsonPropertyName("tarih")]
    public string DateString
    {
        get => _date.ToString("yyyy-MM-dd");
        set => _date = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public DateTime Date
    {
        get => _date;
        set => _date = value;
    }

    [MaxLength(50)]
    [JsonPropertyName("uzuntarih")]
    public string? LongDate { get; set; }
}
