using Microsoft.EntityFrameworkCore;
using MVC_Di.Models;

namespace MVC_Di.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AccountingDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        if (await dbContext.AppUsers.AnyAsync())
        {
            return;
        }

        var demoUser = new AppUser
        {
            Username = "demo",
            Password = "demo123",
            DisplayName = "示範使用者"
        };

        dbContext.AppUsers.Add(demoUser);
        await dbContext.SaveChangesAsync();

        dbContext.UserCategories.AddRange(
            new UserCategory
            {
                Name = "早餐",
                AppUserId = demoUser.Id
            },
            new UserCategory
            {
                Name = "交通",
                AppUserId = demoUser.Id
            });

        await dbContext.SaveChangesAsync();

        dbContext.AccountRecords.AddRange(
            new AccountRecord
            {
                Category = "早餐",
                Description = "豆漿與蛋餅",
                Amount = 85,
                SpendDate = DateTime.Today.AddDays(-1),
                AppUserId = demoUser.Id
            },
            new AccountRecord
            {
                Category = "交通",
                Description = "捷運儲值",
                Amount = 200,
                SpendDate = DateTime.Today,
                AppUserId = demoUser.Id
            });

        await dbContext.SaveChangesAsync();
    }
}
