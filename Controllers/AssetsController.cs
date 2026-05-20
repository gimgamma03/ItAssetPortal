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

    public async Task<IActionResult> Index()
    {
        var assets = await _db.Assets
            .OrderByDescending(a => a.Id)
            .ToListAsync();

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
