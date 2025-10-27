using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Extensions;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Algebra.WebShop2025.App.Controllers;

public class ProductsController(ApplicationDbContext context) : Controller
{
    public IActionResult Index(int? categoryId)
    {
        ViewData["Categories"] = new SelectList(context.Categories, "Id", "Name");

        var cart = HttpContext.Session.GetCart();
        ViewData["CartCount"] = cart?.Items?.Count;

        if (categoryId != null)
        {
            var products = (
                from p in context.Products
                join pc in context.ProductCategories on p.Id equals pc.ProductId
                where pc.CategoryId == categoryId
                select p
            ).ToList();

            return View(products);
        }
        else
        {
            var products = context.Products.ToList();
            return View(products);
        }
    }
}
