using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Extensions;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Algebra.WebShop2025.App.Controllers;

[Authorize]
public class CartController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetCart();

        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart(int productId)
    {
        var cart = HttpContext.Session.GetCart();

        var existingItem = cart.Items.SingleOrDefault(x => x.Product.Id == productId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            var product = context.Products.Find(productId);
            if (product != null)
            {
                cart.Items.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
            }
        }

        HttpContext.Session.SetCart(cart);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var cart = HttpContext.Session.GetCart();

        if (cart.Items.Any(x => x.Product.Id == productId))
        {
            var item = cart.Items.Single(x => x.Product.Id == productId);
            cart.Items.Remove(item);
        }

        HttpContext.Session.SetCart(cart);

        return RedirectToAction(nameof(Index));
    }
}
