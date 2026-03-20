using System.ComponentModel.DataAnnotations;

namespace MVC_Di.Models;

public class CreateRecordViewModel
{
    [Required(ErrorMessage = "請輸入分類")]
    [Display(Name = "分類")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入用途")]
    [Display(Name = "用途")]
    public string Description { get; set; } = string.Empty;

    [Range(1, 999999, ErrorMessage = "金額需大於 0")]
    [Display(Name = "金額")]
    public decimal Amount { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "日期")]
    public DateTime SpendDate { get; set; } = DateTime.Today;
}
