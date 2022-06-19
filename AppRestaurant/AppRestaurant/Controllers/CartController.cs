using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using AppRestaurant.Models;
using AppRestaurant.Models.User;
using AppRestaurant.Models.Forms;
using AppRestaurant.Services;

namespace AppRestaurant.Controllers
{
    public class CartController : Controller
    {
        //carts.userId.dishId.count
        static private Dictionary<int, CartModel> carts = new Dictionary<int, CartModel>();
        public IActionResult Index()
        {
            int? nullTest = HttpContext.Session.GetInt32("userId");

            if (nullTest == null)
            {
                TempData["status"] = "should-be-logged";
                return Redirect("/login");
            }

            int userId = (int)nullTest;

            dynamic models = new ExpandoObject();

            DishService dishService = new DishService();

            if (!carts.ContainsKey(userId))
                carts[userId] = new CartModel();

            Dictionary<int, int> dishesIds = carts[userId].GetDishesIdsAndCount;
            List<Array> dishes = new List<Array>();

            foreach (var dishId in dishesIds)
            {
                dishes.Add(new dynamic[] { dishId.Value, new DishUserModel(dishService.GetOne(dishId.Key)) });
            }

            models.dishes = dishes;
            models.user = new UserService().GetOne(userId);

            return View(models);
        }

        public IActionResult Finalize()
        {
            int? nullTest = HttpContext.Session.GetInt32("userId");

            if (nullTest == null)
            {
                TempData["status"] = "should-be-logged";
                return Redirect("/login");
            }

            int userId = (int)nullTest;

            string[] body = new StreamReader(Request.Body).ReadToEndAsync().Result.Split(", ");
            string email = body[0];
            decimal finalCost;
            Decimal.TryParse(body[1], out finalCost);

            Dictionary<int, int> dishesIds = carts[userId].GetDishesIdsAndCount;

            OrderService orderService = new OrderService();

            orderService.Create(userId, finalCost, dishesIds);
            HttpContext.Session.SetInt32("orderUpdateId", orderService.GetLastId());

            return View();
        }

        public IActionResult Final([FromForm] AddressFormDataModel addressFormDataModel)
        {
            int? nullTest = HttpContext.Session.GetInt32("userId");

            if (nullTest == null)
            {
                TempData["status"] = "should-be-logged";
                return Redirect("/login");
            }

            int userId = (int)nullTest;

            nullTest = HttpContext.Session.GetInt32("orderUpdateId");

            if (nullTest == null)
            {
                return Redirect("/cart");
            }

            int orderId = (int)nullTest;

            HttpContext.Session.Remove("orderUpdateId");

            AddressService addressService = new AddressService();
            addressService.Create(addressFormDataModel);
            new OrderService().PatchAddressFinal(orderId, addressService.GetLastID());
            
            carts[(int)userId].Clear();
            HttpContext.Session.SetString("isEmptyCart", "true");

            return View("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return View();
        }


        public IActionResult Add(int id)
        {
            int? nullTest = HttpContext.Session.GetInt32("userId");

            if (nullTest == null)
            {
                TempData["status"] = "should-be-logged";
                return Redirect("/login");
            }

            int userId = (int)nullTest;

            if (userId < 0)
                return Redirect("login");

            if (!CartController.carts.ContainsKey(userId))
                carts.Add(userId, new CartModel());

            CartController.carts[userId].Add(id);

            HttpContext.Session.SetString("isEmptyCart", "false");

            // If in details page, two clicks are required
            return Redirect(Request.Headers.Referer.ToString());
        }

        public void Remove(int i)
        {
            int? nullTest = HttpContext.Session.GetInt32("userId");

            if (nullTest == null)
                return;

            int userId = (int)nullTest;
            carts[userId].Remove(i);
        }
    }
}
