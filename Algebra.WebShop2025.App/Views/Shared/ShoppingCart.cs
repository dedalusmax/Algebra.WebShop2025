using Algebra.WebShop2025.App.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Algebra.WebShop2025.App.Views.Shared;

//[ViewComponent(Name = "Cart")]
//[NonViewComponent]
public class ShoppingCart : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var cart = HttpContext.Session.GetCart();

        return View(cart);
    }
}
