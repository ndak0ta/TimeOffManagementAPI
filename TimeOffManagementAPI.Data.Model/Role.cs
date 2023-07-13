using System.ComponentModel.DataAnnotations;

namespace TimeOffManagementAPI.Data.Model;

public class Role
{
    [Key]
    public int Id { get; set; }

    [MaxLength(10)]
    [Required]
    public string? Name { get; set; }
}