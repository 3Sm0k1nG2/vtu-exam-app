using AppRestaurant.Data;
using AppRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

using AppRestaurant.Services;

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

            return new UserService().GetAll(); ;
        }
        public UserModel? GetUser()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            string email = new StreamReader(Request.Body).ReadToEndAsync().Result;
            if (email == null)
                return null;

            UserService userService = new UserService();

            int id;
            if (int.TryParse(email, out id))
                return userService.GetOne(id);

            return userService.GetOne(email);
        }

        public void DeleteUser()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return;

            UserService userService = new UserService();

            string email = new StreamReader(Request.Body).ReadToEndAsync().Result;
            if (email == null)
                return;

            int id;
            if (int.TryParse(email, out id))
                userService.Delete(id);
            else
                userService.Delete(email);
        }

        // Dishes CRUD
        public List<DishModel> GetDishes()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            return new DishService().GetAll();
        }

        public DishModel? GetDish()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            int id;
            string result = new StreamReader(Request.Body).ReadToEndAsync().Result;

            if (!int.TryParse(result, out id))
                return null;

            return new DishService().GetOne(id);
        }
        public void DeleteDish()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return;

            int id;
            string result = new StreamReader(Request.Body).ReadToEndAsync().Result;

            if (!int.TryParse(result, out id))
                return;

            new DishService().Delete(id);
        }

        // Orders CRUD

        public List<OrderDetailedModel>? GetOrdersDetailed()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            FilterModel filters = new FilterModel();

            string result = new StreamReader(Request.Body).ReadToEndAsync().Result;

            int id;

            if (int.TryParse(result, out id))
                filters.Id = id;
            else if (result.Contains('@'))
                filters.Email = result;
            else
                filters = null;

            OrdersDB ordersDb = new OrdersDB();
            List<OrderModel>? orders = ordersDb.GetAll(filters);

            return orders;
        }
        public OrderDetailedModel? GetOrderDetailed()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return null;

            OrdersDB ordersDb = new OrdersDB();
            OrderModel? order = null;

            int id;
            string result = new StreamReader(Request.Body).ReadToEndAsync().Result;

            if (int.TryParse(result, out id))
            {
                order = ordersDb.GetOne(id);
            }

            return order;
        }

        public void DeleteOrder()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
                return;

            OrdersDB ordersDb = new OrdersDB();

            int id;
            string result = new StreamReader(Request.Body).ReadToEndAsync().Result;

            if (int.TryParse(result, out id))
            {
                ordersDb.Delete(id);
            }
        }
    }
}
