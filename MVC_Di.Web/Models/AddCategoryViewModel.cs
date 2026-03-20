using System.ComponentModel.DataAnnotations;

namespace MVC_Di.Models;

public class AddCategoryViewModel
{
    [Required(ErrorMessage = "請輸入分類名稱")]
    [StringLength(100, ErrorMessage = "分類名稱最多 100 字")]
    [Display(Name = "新分類")]
    public string Name { get; set; } = string.Empty;
}
