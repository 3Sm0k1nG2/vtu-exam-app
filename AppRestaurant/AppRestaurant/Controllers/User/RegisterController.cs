using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Models;

namespace AppRestaurant.Controllers.User
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            switch (TempData["status"])
            {
                case "not-verified":
                    {
                        MessageModel model = new MessageModel(3, "Предоставената информация не съвпада с дадените критерии!");
                        return View(model);
                    }
            }

            return View();
        }
    }
}
