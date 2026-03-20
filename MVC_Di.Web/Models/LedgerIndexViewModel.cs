namespace MVC_Di.Models;

public class LedgerIndexViewModel
{
    public string DisplayName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public CreateRecordViewModel NewRecord { get; set; } = new();
    public List<AccountRecord> Records { get; set; } = [];
}
