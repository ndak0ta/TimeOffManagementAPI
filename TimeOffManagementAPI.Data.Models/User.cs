using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOffManagementAPI.Data.Model;

public class User
{
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

    [Required]
    public string? Email { get; set; }

    [MaxLength(13)]
    [Required]
    public string? PhoneNumber { get; set; }

    [ForeignKey("Role")]
    [Required]
    public int RoleId { get; set; }

    public Role? Role { get; set; }
}