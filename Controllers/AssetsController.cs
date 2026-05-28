using ItAssetPortal.Data;
using ItAssetPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItAssetPortal.Controllers;

public class AssetsController : Controller
{
    private readonly AppDbContext _db;

    public AssetsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? q, string sort = "latest")
    {
        var query = _db.Assets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var keyword = q.Trim();
            query = query.Where(a =>
                a.Name.Contains(keyword) ||
                (a.SerialNumber != null && a.SerialNumber.Contains(keyword)));
        }

        query = sort switch
        {
            "name" => query.OrderBy(a => a.Name).ThenByDescending(a => a.Id),
            _ => query.OrderByDescending(a => a.Id)
        };

        var assets = await query
            .AsNoTracking()
            .ToListAsync();

        ViewBag.Query = q ?? string.Empty;
        ViewBag.Sort = sort;

        return View(assets);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Asset asset)
    {
        if (!ModelState.IsValid)
        {
            return View(asset);
        }

        asset.CreatedAt = DateTime.UtcNow;
        _db.Assets.Add(asset);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
