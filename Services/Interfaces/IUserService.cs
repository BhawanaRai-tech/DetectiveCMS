using ShadowFile.DTOs;
using ShadowFile.Models;

namespace ShadowFile.Interfaces;

public interface IUserService
{
    Task<User?> AuthenticateAsync(LoginDto loginDto);
    Task<bool> CreateUserAsync(CreateUserDto dto);
}