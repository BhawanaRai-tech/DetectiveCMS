using System.ComponentModel.DataAnnotations;

namespace ShadowFile.Models;

public class User
{
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string AgentCode { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public int RoleId { get; set; }
    public Role? Role { get; set; }
}