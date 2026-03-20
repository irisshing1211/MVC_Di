using Microsoft.EntityFrameworkCore;
using MVC_Di.Data;
using MVC_Di.Models;

namespace MVC_Di.Services;

public class CategoryService(AccountingDbContext dbContext, ILogger<CategoryService> logger) : ICategoryService
{
    public async Task<List<string>> GetCategoriesAsync(int userId)
    {
        return await dbContext.UserCategories
            .Where(category => category.AppUserId == userId)
            .OrderBy(category => category.Name)
            .Select(category => category.Name)
            .ToListAsync();
    }

    public async Task<bool> CategoryExistsAsync(int userId, string categoryName)
    {
        var normalizedCategoryName = categoryName.Trim();

        return await dbContext.UserCategories.AnyAsync(category =>
            category.AppUserId == userId && category.Name == normalizedCategoryName);
    }

    public async Task<bool> AddCategoryAsync(int userId, AddCategoryViewModel input)
    {
        var normalizedCategoryName = input.Name.Trim();

        if (await CategoryExistsAsync(userId, normalizedCategoryName))
        {
            return false;
        }

        dbContext.UserCategories.Add(new UserCategory
        {
            AppUserId = userId,
            Name = normalizedCategoryName
        });

        await dbContext.SaveChangesAsync();
        logger.LogInformation("Category created for user {UserId}: {Category}", userId, normalizedCategoryName);
        return true;
    }
}
