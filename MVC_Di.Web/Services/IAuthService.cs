using MVC_Di.Models;

namespace MVC_Di.Services;

public interface IAuthService
{
    Task<AppUser?> ValidateUserAsync(string username, string password);
    Task<bool> UsernameExistsAsync(string username);
    Task<AppUser?> RegisterUserAsync(RegisterViewModel input);
}
