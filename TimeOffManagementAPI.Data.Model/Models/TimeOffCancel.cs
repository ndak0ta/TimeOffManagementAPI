using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOffManagementAPI.Data.Model.Models;

public class TimeOffCancel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("TimeOff")]
    public int TimeOffId { get; set; }

    [Required]
    [ForeignKey("User")]
    public string? UserId { get; set; }

    public bool IsApproved { get; set; }

    public bool IsPending { get; set; }
}
