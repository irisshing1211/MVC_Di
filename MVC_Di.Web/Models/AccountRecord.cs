using System.ComponentModel.DataAnnotations;

namespace MVC_Di.Models;

public class AccountRecord
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 999999)]
    public decimal Amount { get; set; }

    [DataType(DataType.Date)]
    public DateTime SpendDate { get; set; } = DateTime.Today;

    public int AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
