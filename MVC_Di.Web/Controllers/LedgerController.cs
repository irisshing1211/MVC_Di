using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Di.Models;
using MVC_Di.Services;

namespace MVC_Di.Controllers;

[Authorize]
public class LedgerController(IRecordService recordService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var records = await recordService.GetRecordsAsync(userId);

        var viewModel = new LedgerIndexViewModel
        {
            DisplayName = User.FindFirst("DisplayName")?.Value ?? User.Identity?.Name ?? "使用者",
            Records = records,
            TotalAmount = records.Sum(record => record.Amount),
            NewRecord = new CreateRecordViewModel { SpendDate = DateTime.Today }
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRecordViewModel input)
    {
        if (!ModelState.IsValid)
        {
            var userId = GetUserId();
            var records = await recordService.GetRecordsAsync(userId);

            return View("Index", new LedgerIndexViewModel
            {
                DisplayName = User.FindFirst("DisplayName")?.Value ?? User.Identity?.Name ?? "使用者",
                Records = records,
                TotalAmount = records.Sum(record => record.Amount),
                NewRecord = input
            });
        }

        await recordService.AddRecordAsync(GetUserId(), input);
        return RedirectToAction(nameof(Index));
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
