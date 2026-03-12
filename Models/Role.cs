using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ShadowFile.Models;

public class Role
{
    public int Id { get; set; }

    [Required]
    public string RoleName { get; set; } = string.Empty;

    public ICollection<User>? Users { get; set; }

    public HelperResult Name(object arg)
    {
        throw new NotImplementedException();
    }
}