using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Algebra.WebShop2025.App.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OrderItemsController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/OrderItems
    public async Task<IActionResult> Index()
    {
        return View(await _context.OrderItems.ToListAsync());
    }

    // GET: Admin/OrderItems/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderItem = await _context.OrderItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (orderItem == null)
        {
            return NotFound();
        }

        return View(orderItem);
    }

    // GET: Admin/OrderItems/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/OrderItems/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,OrderId,ProductId,Quantity,Price,Total")] OrderItem orderItem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(orderItem);
    }

    // GET: Admin/OrderItems/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }
        return View(orderItem);
    }

    // POST: Admin/OrderItems/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ProductId,Quantity,Price,Total")] OrderItem orderItem)
    {
        if (id != orderItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(orderItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(orderItem.Id))
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
        return View(orderItem);
    }

    // GET: Admin/OrderItems/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderItem = await _context.OrderItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (orderItem == null)
        {
            return NotFound();
        }

        return View(orderItem);
    }

    // POST: Admin/OrderItems/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem != null)
        {
            _context.OrderItems.Remove(orderItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrderItemExists(int id)
    {
        return _context.OrderItems.Any(e => e.Id == id);
    }
}
