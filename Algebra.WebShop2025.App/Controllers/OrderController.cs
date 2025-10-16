using Algebra.WebShop2025.App.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Algebra.WebShop2025.App.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetCart();

            ViewData["Cart"] = cart;

            return View();
        }
    }
}
