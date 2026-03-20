using Microsoft.EntityFrameworkCore;
using MVC_Di.Data;
using MVC_Di.Models;

namespace MVC_Di.Services;

public class CustomRecordService(AccountingDbContext dbContext, ILogger<CustomRecordService> logger) : RecordService(dbContext, logger)
{
    public override async Task<List<AccountRecord>> GetRecordsAsync(int userId)
    {
        return await dbContext.AccountRecords.Where(record => record.AppUserId == userId)
                              .OrderByDescending(record => record.SpendDate)
                              .ThenByDescending(record => record.Amount)
                              .ToListAsync();
    }
}
