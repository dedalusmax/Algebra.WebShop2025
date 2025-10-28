using Algebra.WebShop2025.App.Areas.Admin.Models;
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
            );

            var viewModel = products.Select(product => new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                FileName = product.FileName,
                FileContentBase64 = product.FileContent != null ? Convert.ToBase64String(product.FileContent) : null
            }).ToList();

            return View(viewModel);
        }
        else
        {
            var products = context.Products;

            var viewModel = products.Select(product => new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                FileName = product.FileName,
                FileContentBase64 = product.FileContent != null ? Convert.ToBase64String(product.FileContent) : null
            }).ToList();

            return View(viewModel);
        }
    }
}
