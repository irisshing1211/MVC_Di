using Microsoft.EntityFrameworkCore;
using MVC_Di.Data;
using MVC_Di.Models;

namespace MVC_Di.Services;

public class RecordService(AccountingDbContext dbContext, ILogger<RecordService> logger) : IRecordService
{
    public virtual async Task<List<AccountRecord>> GetRecordsAsync(int userId)
    {
        return await dbContext.AccountRecords
            .Where(record => record.AppUserId == userId)
            .OrderByDescending(record => record.SpendDate)
            .ThenByDescending(record => record.Id)
            .ToListAsync();
    }


    public virtual async Task AddRecordAsync(int userId, CreateRecordViewModel input)
    {
        var record = new AccountRecord
        {
            AppUserId = userId,
            Category = input.Category,
            Description = input.Description,
            Amount = input.Amount,
            SpendDate = input.SpendDate
        };

        dbContext.AccountRecords.Add(record);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Record created for user {UserId}: {Category} {Amount}", userId, input.Category, input.Amount);
    }
}
