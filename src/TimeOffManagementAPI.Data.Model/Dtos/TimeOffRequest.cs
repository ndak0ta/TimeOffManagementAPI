using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class TimeOffRequest
{
    public TimeOffRequest()
    {
        TotalDays = (EndDate - StartDate).Days;
    }

    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public int TotalDays { get; set; }

    [Required]
    public string? UserId { get; set; }
}