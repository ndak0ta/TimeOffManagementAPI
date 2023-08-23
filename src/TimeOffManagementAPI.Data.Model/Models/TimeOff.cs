using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOffManagementAPI.Data.Model.Models;

public class TimeOff
{
    public TimeOff()
    {
        IsApproved = false;
        IsPending = true;
        IsActive = true;
        HasCancelRequest = false;
        IsCancelled = false;
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

    public bool IsApproved { get; set; }

    public bool IsPending { get; set; }

    public bool IsActive { get; set; }
    public bool HasCancelRequest { get; set; }

    public bool IsCancelled { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("User")]
    public string? UserId { get; set; }
}