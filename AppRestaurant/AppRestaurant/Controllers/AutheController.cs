using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Data;
using AppRestaurant.Models;
using AppRestaurant.Models.Forms;

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

            UsersDB userDB = new UsersDB();
            UserModel model = new UserModel(formData);
            model = userDB.Connect(model);

            if (model == null)
            {
                TempData["status"] = "wrong-credentials";
                return Redirect("/login");
            }


            String nickname;

            if (model.Nickname == null)
                nickname = model.Email.Split("@")[0];
            else
                nickname = model.Nickname;

            HttpContext.Session.SetString("Nickname", nickname);
            HttpContext.Session.SetString("Email", model.Email);

            if (AdminController.isAdmin(model.Email))
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

            UsersDB userDB = new UsersDB();

            if (userDB.Exists(formData.Email))
            {
                TempData["status"] = "existing";
                return Redirect("/login");
            }

            UserModel model;

            if (formData.Nickname != null)
                model = new UserModel(formData);
            else
                model = new UserModel(formData);

            userDB.Add(model);
            TempData["status"] = "success";

            return Redirect("/login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("isAdmin");
            HttpContext.Session.Remove("Nickname");
            HttpContext.Session.Remove("Email");

            return Redirect("/");
        }
    }
}
