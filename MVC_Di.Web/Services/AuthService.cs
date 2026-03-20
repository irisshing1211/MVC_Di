using Microsoft.EntityFrameworkCore;
using MVC_Di.Data;
using MVC_Di.Models;

namespace MVC_Di.Services;

public class AuthService(AccountingDbContext dbContext, ILogger<AuthService> logger) : IAuthService
{
    public async Task<AppUser?> ValidateUserAsync(string username, string password)
    {
        var user = await dbContext.AppUsers
            .FirstOrDefaultAsync(item => item.Username == username && item.Password == password);

        if (user is null)
        {
            logger.LogWarning("Login failed for username {Username}", username);
            return null;
        }

        logger.LogInformation("Login succeeded for username {Username}", username);
        return user;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var normalizedUsername = username.Trim();
        return await dbContext.AppUsers.AnyAsync(item => item.Username == normalizedUsername);
    }

    public async Task<AppUser?> RegisterUserAsync(RegisterViewModel input)
    {
        var user = new AppUser
        {
            Username = input.Username.Trim(),
            Password = input.Password,
            DisplayName = input.DisplayName.Trim()
        };

        dbContext.AppUsers.Add(user);

        try
        {
            await dbContext.SaveChangesAsync();
            logger.LogInformation("User {Username} registered", user.Username);
            return user;
        }
        catch (DbUpdateException exception)
        {
            logger.LogWarning(exception, "Registration failed for username {Username}", user.Username);
            return null;
        }
    }
}
