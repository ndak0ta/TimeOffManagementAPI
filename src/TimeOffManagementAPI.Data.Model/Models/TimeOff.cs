using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeOffManagementAPI.Data.Model.Constants;

namespace TimeOffManagementAPI.Data.Model.Models;

public class TimeOff
{
    public TimeOff()
    {
        Status = TimeOffStates.Pending;
        IsActive = true;
        CreatedAt = DateTime.Now;
        TotalDays = (EndDate - StartDate).Days;
    }

    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public int TotalDays { get; set; }
    public string? Status { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("User")]
    public string? UserId { get; set; }
}