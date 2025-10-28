using Algebra.WebShop2025.App.Areas.Admin.Models;
using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Algebra.WebShop2025.App.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Products
    //[AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        //return View(await _context.Products.ToListAsync());

        var result = await _context.Products
            .Include(p => p.Categories) // entity: Product categories
            .ThenInclude(pc => pc.Category)
            .ToListAsync();

        return View(result);    
    }

    // GET: Admin/Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        var viewModel = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            FileName = product.FileName,
            FileContentBase64 = product.FileContent != null ? Convert.ToBase64String(product.FileContent) : null
        };

        return View(viewModel);
    }

    // GET: Admin/Products/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Products/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
    {
        ModelState.Remove(nameof(Product.Categories));
        ModelState.Remove(nameof(Product.OrderItems));
        ModelState.Remove(nameof(Product.FileName));
        ModelState.Remove(nameof(Product.FileContent));

        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: Admin/Products/Edit/5
    [Authorize(Policy = "RequireAdminWithCredit")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: Admin/Products/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, IFormFile? file, [Bind("Id,Name,Price,Description")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(Product.Categories));
        ModelState.Remove(nameof(Product.OrderItems));
        ModelState.Remove(nameof(Product.FileName));
        ModelState.Remove(nameof(Product.FileContent));

        if (ModelState.IsValid)
        {
            if (file != null && file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                product.FileName = file.FileName;
                product.FileContent = memoryStream.ToArray();
            }

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
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
        return View(product);
    }

    // GET: Admin/Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Admin/Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
