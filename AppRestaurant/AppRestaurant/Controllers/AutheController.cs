using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Data;
using AppRestaurant.Models;
using AppRestaurant.Models.Forms;
using AppRestaurant.Services;

namespace AppRestaurant.Controllers
{
    public class AutheController : Controller
    {
        [HttpPost]
        public IActionResult Login([FromForm] UserFormDataModel.LoginModel formData)
        {
            if (!formData.IsVerified())
            {
                TempData["status"] = "not-verified";
                return Redirect("/login");
            }

            UserModel user = new UserService().Login(formData);

            if (user == null)
            {
                TempData["status"] = "wrong-credentials";
                return Redirect("/login");
            }

            string nickname;

            if (user.Nickname == null)
                nickname = user.Email.Split("@")[0];
            else
                nickname = user.Nickname;

            HttpContext.Session.SetInt32("userId", user.Id);
            HttpContext.Session.SetString("Nickname", nickname);
            HttpContext.Session.SetString("Email", user.Email);

            if (AdminController.isAdmin(user.Email))
                HttpContext.Session.SetString("isAdmin", "true");
            else
                HttpContext.Session.SetString("isAdmin", "false");

            HttpContext.Session.SetString("isEmptyCart", "true");

            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Reg([FromForm] UserFormDataModel.RegModel formData)
        {
            if (formData == null)
            {
                return BadRequest();
            }

            if (!formData.IsVerified())
            {
                TempData["status"] = "not-verified";
                return Redirect("/register");
            }

            UserService userService = new UserService();

            if (userService.Exists(formData.Email))
            {
                TempData["status"] = "existing";
                return Redirect("/login");
            }

            userService.Create(formData);
            TempData["status"] = "success";

            return Redirect("/login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("isAdmin");
            HttpContext.Session.Remove("Nickname");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("isEmptyCart");
            HttpContext.Session.Remove("updateFormId");

            return Redirect("/");
        }
    }
}
