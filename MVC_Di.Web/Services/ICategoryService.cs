using MVC_Di.Models;

namespace MVC_Di.Services;

public interface ICategoryService
{
    Task<List<string>> GetCategoriesAsync(int userId);
    Task<bool> CategoryExistsAsync(int userId, string categoryName);
    Task<bool> AddCategoryAsync(int userId, AddCategoryViewModel input);
}
