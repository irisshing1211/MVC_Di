using System.ComponentModel.DataAnnotations;

namespace MVC_Di.Models;

public class UserCategory
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public int AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
