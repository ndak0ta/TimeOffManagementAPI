using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOffManagementAPI.Data.Model;

public class Role
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
}