using MVC_Di.Models;

namespace MVC_Di.Services;

public interface IRecordService
{
    Task<List<AccountRecord>> GetRecordsAsync(int userId);
    Task AddRecordAsync(int userId, CreateRecordViewModel input);
}
