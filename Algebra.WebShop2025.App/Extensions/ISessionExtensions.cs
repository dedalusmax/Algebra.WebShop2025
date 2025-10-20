using Algebra.WebShop2025.App.Models;
using Newtonsoft.Json;

namespace Algebra.WebShop2025.App.Extensions;

public static class ISessionExtensions
{
    private const string CART_SESSION_KEY = "_cart";

    public static void SetCart(this ISession session, Cart cart)
    {
        var sessionData = JsonConvert.SerializeObject(cart);
        session.SetString(CART_SESSION_KEY, sessionData);
    }

    public static Cart GetCart(this ISession session)
    {
        var sessionData = session.GetString(CART_SESSION_KEY);
        return string.IsNullOrEmpty(sessionData) ? new Cart() : JsonConvert.DeserializeObject<Cart>(sessionData)!;
    }

    public static void ClearCart(this ISession session)
    {
        session.Remove(CART_SESSION_KEY);
        //session.SetString(CART_SESSION_KEY, string.Empty);
    }
}
