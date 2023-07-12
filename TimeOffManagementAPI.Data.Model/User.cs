using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOffManagementAPI.Data.Model;

public class User
{
    public User()
    {
        AnnualTimeOffs = 14;
    }

    [Key]
    public int Id { get; set; }

    [MaxLength(15)]
    [Required]
    public string? Username { get; set; }

    [MaxLength(20)]
    [Required]
    public string? Password { get; set; }

    [MaxLength(15)]
    [Required]
    public string? FirstName { get; set; }

    [MaxLength(15)]
    [Required]
    public string? LastName { get; set; }

    [MaxLength(320)]
    [Required]
    public string? Email { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [MaxLength(13)]
    [Required]
    public string? PhoneNumber { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [Required]
    public int AnnualTimeOffs { get; set; }

    [MaxLength(10)]
    [Required]
    public string? Role { get; set; }
}