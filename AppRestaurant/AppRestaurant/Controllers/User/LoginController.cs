using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Models;

namespace AppRestaurant.Controllers.User
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            MessageModel? model = null;

            switch (TempData["status"])
            {
                case "success":
                    {
                        model = new MessageModel(2, "Успешно регистриран потребител!");
                        break;
                    }
                case "existing":
                    {
                        model = new MessageModel(4, "Емайлът е зает! Пренасочване към страницата за вход.");
                        break;
                    }
                case "not-verified":
                    {
                        model = new MessageModel(3, "Предоставената информация е недостатъчна!");
                        break;
                    }
                case "wrong-credentials":
                    {
                        model = new MessageModel(3, "Грешни входни данни!");
                        break;
                    }
                case "should-be-logged":
                    {
                        model = new MessageModel("За да изпълните съответната операция, трябва да сте потребител.");
                        break;
                    }
            }

            return View(model);
        }
    }
}
