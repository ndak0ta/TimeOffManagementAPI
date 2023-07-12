using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOffManagementAPI.Data.Model;

public class TimeOffRequest
{
    public TimeOffRequest()
    {
        isApproved = false;
    }

    [Key]
    public int Id { get; set; } 
    
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string? Description { get; set; }

    public bool isApproved { get; set; }

    [ForeignKey("User")]
    public int userId { get; set; } 

    public User? User { get; set; }
}