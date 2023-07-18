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
        CreatedAt = DateTime.Now;
    }

    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public bool IsApproved { get; set; }

    public bool IsPending { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("User")]
    public string? UserId { get; set; }

    public User? User { get; set; }
}