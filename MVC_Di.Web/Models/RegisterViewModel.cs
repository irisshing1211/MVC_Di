using System.ComponentModel.DataAnnotations;

namespace MVC_Di.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "請輸入顯示名稱")]
    [StringLength(50, ErrorMessage = "顯示名稱最多 50 字")]
    [Display(Name = "顯示名稱")]
    public string DisplayName { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入帳號")]
    [StringLength(50, ErrorMessage = "帳號最多 50 字")]
    [Display(Name = "帳號")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入密碼")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼長度需介於 6 到 100 字")]
    [DataType(DataType.Password)]
    [Display(Name = "密碼")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "請再次輸入密碼")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "兩次輸入的密碼不一致")]
    [Display(Name = "確認密碼")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
}
