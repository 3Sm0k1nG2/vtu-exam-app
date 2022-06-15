using AppRestaurant.Data;
using AppRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

using System.Diagnostics;

// user is able to make request to other properties. Index blocks only for itself
// cheated solution, return nothing is user is not admin,
// but u don't get response from server
// => indicates that there is something behind this url request.

namespace AppRestaurant.Controllers
{
    public class AdminController : Controller
    {
        static private readonly string ADMIN_MAIL = "D33C3BDE98DC60225114F43BC74301AB";
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return NotFound();

            return View();
        }

        static public bool isAdmin(string email)
        {
            return Utils.MD5_Algorithm.CalculateMD5Hash(email) == ADMIN_MAIL;
        }


        // User CRUD
        public List<UserModel> GetUsers()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            dynamic models = new ExpandoObject();

            UsersDB usersDB = new UsersDB();
            List<UserModel> users = usersDB.GetAll();

            return users;
        }
        public UserModel GetUser()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            UsersDB usersDB = new UsersDB();
            UserModel? user;

            int id;
            string email = new StreamReader(Request.Body).ReadToEndAsync().Result;

            if (int.TryParse(email, out id))
            {
                user = usersDB.GetOne(id);
                return user;
            }

            user = usersDB.GetOne(email);
            return user;
        }

        public void DeleteUser()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return;

            UsersDB usersDB = new UsersDB();

            int id;
            string email = new StreamReader(Request.Body).ReadToEndAsync().Result;

            if (int.TryParse(email, out id))
                usersDB.Delete(id);
            else
                usersDB.Delete(email);
        }

        // Dishes CRUD
        public List<DishModel> GetDishes()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            DishesDb dishesDb = new DishesDb();
            List<DishModel>? dishes = dishesDb.GetAll();

            return dishes;
        }

        public DishModel GetDish()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            DishesDb dishesDb = new DishesDb();
            DishModel? dish = null;

            int id;
            string result = new StreamReader(Request.Body).ReadToEndAsync().Result;

            if (int.TryParse(result, out id))
            {
                dish = dishesDb.GetOne(id);
            }

            return dish;
        }
        public void DeleteDish()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return;

            DishesDb dishesDb = new DishesDb();

            int id;
            string result = new StreamReader(Request.Body).ReadToEndAsync().Result;

            if (int.TryParse(result, out id))
            {
                dishesDb.Delete(id);
            }
        }

        // To implement
        // Orders CRUD

        public List<OrderModel> GetAllOrders() { return null; }
        public void Delete() { }
    }
}
