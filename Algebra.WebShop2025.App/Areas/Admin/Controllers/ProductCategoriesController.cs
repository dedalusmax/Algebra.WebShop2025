using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Algebra.WebShop2025.App.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductCategoriesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductCategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/ProductCategories
    public async Task<IActionResult> Index(int productId)
    {
        var applicationDbContext = _context.ProductCategories
            .Include(p => p.Category)
            .Include(p => p.Product)
            .Where(x => x.ProductId == productId);

        ViewBag.ProductId = productId;

        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Admin/ProductCategories/Details/5
    public async Task<IActionResult> Details(int? id, int productId)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productCategory = await _context.ProductCategories
            .Include(p => p.Category)
            .Include(p => p.Product)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (productCategory == null)
        {
            return NotFound();
        }

        ViewBag.ProductId = productId;

        return View(productCategory);
    }

    // GET: Admin/ProductCategories/Create
    public IActionResult Create(int productId)
    {
        ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
        ViewData["Products"] = new SelectList(_context.Products.Where(x => x.Id == productId), "Id", "Name");

        ViewBag.ProductId = productId;

        return View();
    }

    // POST: Admin/ProductCategories/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ProductId,CategoryId")] ProductCategory productCategory)
    {
        ModelState.Remove("Product");
        ModelState.Remove("Category");

        if (ModelState.IsValid)
        {
            _context.Add(productCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { productId = productCategory.ProductId });
        }

        ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", productCategory.CategoryId);
        ViewData["Products"] = new SelectList(_context.Products, "Id", "Name", productCategory.ProductId);

        return View(productCategory);
    }

    // GET: Admin/ProductCategories/Edit/5
    public async Task<IActionResult> Edit(int? id, int productId)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productCategory = await _context.ProductCategories.FindAsync(id);
        if (productCategory == null)
        {
            return NotFound();
        }

        ViewBag.ProductId = productId;

        ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", productCategory.CategoryId);
        ViewData["Products"] = new SelectList(_context.Products, "Id", "Name", productCategory.ProductId);

        return View(productCategory);
    }

    // POST: Admin/ProductCategories/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,CategoryId")] ProductCategory productCategory)
    {
        if (id != productCategory.Id)
        {
            return NotFound();
        }

        ModelState.Remove("Product");
        ModelState.Remove("Category");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(productCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(productCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", productCategory.CategoryId);
        ViewData["Products"] = new SelectList(_context.Products, "Id", "Name", productCategory.ProductId);

        return View(productCategory);
    }

    // GET: Admin/ProductCategories/Delete/5
    public async Task<IActionResult> Delete(int? id, int productId)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productCategory = await _context.ProductCategories
            .Include(p => p.Category)
            .Include(p => p.Product)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (productCategory == null)
        {
            return NotFound();
        }

        ViewBag.ProductId = productId;

        return View(productCategory);
    }

    // POST: Admin/ProductCategories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var productCategory = await _context.ProductCategories.FindAsync(id);
        if (productCategory != null)
        {
            _context.ProductCategories.Remove(productCategory);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductCategoryExists(int id)
    {
        return _context.ProductCategories.Any(e => e.Id == id);
    }
}
