using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Di.Models;
using MVC_Di.Services;

namespace MVC_Di.Controllers;

[Authorize]
public class LedgerController(IRecordService recordService, ICategoryService categoryService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        return View(await BuildIndexViewModelAsync(userId));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(Prefix = "NewRecord")] CreateRecordViewModel input)
    {
        var userId = GetUserId();
        var categoryExists = await categoryService.CategoryExistsAsync(userId, input.Category);

        if (!categoryExists)
        {
            ModelState.AddModelError("Category", "請先建立分類，再新增記錄");
        }

        if (!ModelState.IsValid)
        {
            return View("Index", await BuildIndexViewModelAsync(userId, input));
        }

        await recordService.AddRecordAsync(userId, input);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCategory([Bind(Prefix = "NewCategory")] AddCategoryViewModel input)
    {
        var userId = GetUserId();

        if (!ModelState.IsValid)
        {
            if (IsAjaxRequest())
            {
                return BadRequest(new
                {
                    errorMessage = GetFirstModelError() ?? "分類名稱格式不正確"
                });
            }

            return View("Index", await BuildIndexViewModelAsync(userId, newCategory: input));
        }

        var created = await categoryService.AddCategoryAsync(userId, input);
        if (!created)
        {
            if (IsAjaxRequest())
            {
                return Conflict(new
                {
                    errorMessage = "分類名稱已存在"
                });
            }

            ModelState.AddModelError("NewCategory.Name", "分類名稱已存在");
            return View("Index", await BuildIndexViewModelAsync(userId, newCategory: input));
        }

        if (IsAjaxRequest())
        {
            var categories = await categoryService.GetCategoriesAsync(userId);
            return Ok(new
            {
                categoryName = input.Name.Trim(),
                categories
            });
        }

        return RedirectToAction(nameof(Index));
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private bool IsAjaxRequest()
    {
        return Request.Headers.XRequestedWith == "XMLHttpRequest";
    }

    private string? GetFirstModelError()
    {
        return ModelState.Values
            .SelectMany(value => value.Errors)
            .Select(error => error.ErrorMessage)
            .FirstOrDefault(message => !string.IsNullOrWhiteSpace(message));
    }

    private async Task<LedgerIndexViewModel> BuildIndexViewModelAsync(
        int userId,
        CreateRecordViewModel? newRecord = null,
        AddCategoryViewModel? newCategory = null)
    {
        var records = await recordService.GetRecordsAsync(userId);
        var categories = await categoryService.GetCategoriesAsync(userId);

        return new LedgerIndexViewModel
        {
            DisplayName = User.FindFirst("DisplayName")?.Value ?? User.Identity?.Name ?? "使用者",
            Records = records,
            Categories = categories,
            TotalAmount = records.Sum(record => record.Amount),
            NewRecord = newRecord ?? new CreateRecordViewModel { SpendDate = DateTime.Today },
            NewCategory = newCategory ?? new AddCategoryViewModel()
        };
    }
}
