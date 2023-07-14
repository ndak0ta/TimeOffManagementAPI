using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOffManagementAPI.Data.Model.Models;

public class TimeOffRequest
{
    public TimeOffRequest()
    {
        isApproved = false;
    }

    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public bool isApproved { get; set; }

    [ForeignKey("User")]
    public string? userId { get; set; }

    public User? User { get; set; }
}