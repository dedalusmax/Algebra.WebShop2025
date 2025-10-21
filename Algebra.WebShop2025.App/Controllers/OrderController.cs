using Algebra.WebShop2025.App.Data;
using Algebra.WebShop2025.App.Extensions;
using Algebra.WebShop2025.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Algebra.WebShop2025.App.Controllers;

[Authorize]
public class OrderController(ApplicationDbContext context) : Controller
{
    public IActionResult Index(bool? success)
    {
        ViewData["Success"] = success ?? false;

        var cart = HttpContext.Session.GetCart();

        ViewData["Cart"] = cart;

        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = context.Users.Find(userId);

        var order = new Order();
        if (user != null)
        {
            order.CustomerFirstName = string.IsNullOrEmpty(user.FirstName) ? string.Empty : user.FirstName;
            order.CustomerLastName = string.IsNullOrEmpty(user.LastName) ? string.Empty : user.LastName;
            order.CustomerEmailAddress = string.IsNullOrEmpty(user.Email) ? string.Empty : user.Email;
            order.CustomerPhoneNumber = string.IsNullOrEmpty(user.PhoneNumber) ? string.Empty : user.PhoneNumber;
            order.CustomerAddress = string.IsNullOrEmpty(user.Address) ? string.Empty : user.Address;
        }

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder([Bind("CustomerFirstName,CustomerLastName,CustomerEmailAddress,CustomerPhoneNumber,CustomerAddress")] Order order)
    {
        ModelState.Remove("Items");
        ModelState.Remove("UserId");
        ModelState.Remove("User");

        // custom validation! 

        if (ModelState.IsValid)
        {
            // add transaction here later!

            var cart = HttpContext.Session.GetCart();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            order.Total = cart.GrandTotal;
            order.DateTimeCreated = DateTime.UtcNow;
            order.UserId = userId!;

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
            var errors = new List<string>();
            foreach (var item in ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    //Console.WriteLine(error.ErrorMessage);
                    errors.Add(error.ErrorMessage);
                }
            }

            return RedirectToAction(nameof(Index), new { success = false, errors });
        }

        // return View("Error.cshtml");
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
