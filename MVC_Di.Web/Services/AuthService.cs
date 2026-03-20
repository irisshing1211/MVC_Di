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
}
