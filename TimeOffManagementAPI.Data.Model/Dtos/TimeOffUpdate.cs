using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model.Dtos;

public class TimeOffUpdate
{
    [Required]
    public int Id { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public string? UserId { get; set; }
}