using Algebra.WebShop2025.App.Data;
using Microsoft.AspNetCore.Mvc;

namespace Algebra.WebShop2025.App.Controllers;

public class ProductsController(ApplicationDbContext context) : Controller
{
    public IActionResult Index()
    {
        var products = context.Products.ToList();

        return View(products);
    }
}
