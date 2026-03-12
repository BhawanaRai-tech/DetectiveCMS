using System.ComponentModel.DataAnnotations;

namespace ShadowFile.DTOs;

public class LoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string AgentCode { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }
}