using Microsoft.AspNetCore.Mvc;

namespace AppRestaurant.Controllers.User
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/authe/logout");
        }
    }
}
