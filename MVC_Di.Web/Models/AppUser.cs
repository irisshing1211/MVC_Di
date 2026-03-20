using System.ComponentModel.DataAnnotations;

namespace MVC_Di.Models;

public class AppUser
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Password { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string DisplayName { get; set; } = string.Empty;

    public List<AccountRecord> AccountRecords { get; set; } = [];
}
