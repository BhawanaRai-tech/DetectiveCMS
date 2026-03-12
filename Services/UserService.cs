using Microsoft.EntityFrameworkCore;
using ShadowFile.Data;
using ShadowFile.DTOs;
using ShadowFile.Interfaces;
using ShadowFile.Models;

namespace ShadowFile.Services;

public class UserService : IUserService
{
    private readonly ShadowFileDbContext _context;
    private readonly IPasswordProvider _passwordProvider;

    public UserService(ShadowFileDbContext context,
        IPasswordProvider passwordProvider)
    {
        _context = context;
        _passwordProvider = passwordProvider;
    }

    public async Task<User?> AuthenticateAsync(LoginDto loginDto)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u =>
                u.Username == loginDto.Username &&
                u.AgentCode == loginDto.AgentCode &&
                u.RoleId == loginDto.RoleId &&
                u.IsActive);

        if (user == null)
            return null;

        var isValid = _passwordProvider.VerifyPassword(loginDto.Password, user.PasswordHash);

        return isValid ? user : null;
    }

    public async Task<bool> CreateUserAsync(CreateUserDto dto)
    {
        var hashedPassword = _passwordProvider.HashPassword(dto.Password);
        

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            PasswordHash = hashedPassword,
            RoleId = dto.RoleId,
            AgentCode = dto.AgentCode
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateAgentCode()
    {
        return "AGT-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
    }
}