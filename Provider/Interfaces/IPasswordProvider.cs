namespace ShadowFile.Interfaces;

public interface IPasswordProvider
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}