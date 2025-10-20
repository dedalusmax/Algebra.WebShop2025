using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Extensions;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Algebra.WebShop2025.App.Controllers;

[Authorize]
public class OrderController(ApplicationDbContext context) : Controller
{
    public IActionResult Index(bool? success)
    {
        ViewData["Success"] = success ?? false;

        var cart = HttpContext.Session.GetCart();

        ViewData["Cart"] = cart;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder([Bind("CustomerFirstName,CustomerLastName,CustomerEmailAddress,CustomerPhoneNumber,CustomerAddress")] Order order)
    {
        ModelState.Remove("Items");

        // custom validation! 

        if (ModelState.IsValid)
        {
            // add transaction here later!
            var cart = HttpContext.Session.GetCart();

            order.Total = cart.GrandTotal;

            context.Orders.Add(order);
            context.SaveChanges();

            if (cart.Items.Count > 0)
            {
                foreach (var cartItem in cart.Items)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = cartItem.Product.Id,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Product.Price,
                        Total = cartItem.Total
                    };

                    //order.Items.Add(orderItem);
                    context.OrderItems.Add(orderItem);
                }

                context.SaveChanges();
            }

            HttpContext.Session.ClearCart();

            return RedirectToAction(nameof(Index), new { success = true });
        }
        else
        {
            foreach (var item in ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
        }

        return View("Error.cshtml");
    }
}
