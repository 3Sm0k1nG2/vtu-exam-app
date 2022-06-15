using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using AppRestaurant.Data;
using AppRestaurant.Models;
using AppRestaurant.Models.User;
using AppRestaurant.Models.Forms;
using System.Text;

namespace AppRestaurant.Controllers
{
    public class CartController : Controller
    {
        //carts.userId.dishId.count

        static private Dictionary<int, CartModel> carts = new Dictionary<int, CartModel>();
        public IActionResult Index()
        {
            // Many errors here
            // OrdersDB, OrderModel, AddressDb, AddressModel not implemented

            // To implement
            if (HttpContext.Session.GetString("Email") == null)
            {
                TempData["status"] = "should-be-logged";
                return Redirect("/login");
            }

            dynamic models = new ExpandoObject();

            models.user = new UsersDB().GetOne(HttpContext.Session.GetString("Email"));
            DishesDb dishDb = new DishesDb();

            if (!carts.ContainsKey(models.user.Id))
                carts[models.user.Id] = new CartModel();

            Dictionary<int, int> dishesIds = carts[models.user.Id].GetDishesIdsAndCount;

            List<Array> dishes = new List<Array>();

            foreach (var dishId in dishesIds)
            {
                dishes.Add(new dynamic[] { dishId.Value, new DishUserModel(dishDb.GetOne(dishId.Key)) });
            }

            models.dishes = dishes;

            return View(models);
        }

        //[HttpPost]
        public IActionResult Finalize()
        {
            // Many errors here
            // OrdersDB, OrderModel, AddressDb, AddressModel not implemented

            string[] body = new StreamReader(Request.Body).ReadToEndAsync().Result.Split(", ");
            string email = body[0];
            decimal finalCost;
            Decimal.TryParse(body[1], out finalCost);

            int userId = new UsersDB().GetId(email);

            OrdersDB orderDb = new OrdersDB();
            OrderModel order = new OrderModel();

            Dictionary<int, int> dishesIds = carts[userId].GetDishesIdsAndCount;
            StringBuilder dishes = new StringBuilder();

            // Quantity x Id
            foreach (var dish in dishesIds)
            {
                dishes.Append($"{dish.Value}x{dish.Key},");
            }

            dishes.Remove(dishes.Length-1, 1);

            order.UserId = userId;
            order.DishesIdsString = dishes.ToString();
            order.Status = "pending";
            order.Cost = finalCost;

            orderDb.Create(order);
            order.Id = OrdersDB.lastId;

            return View(order.Id);
        }

        public IActionResult Final([FromForm] AddressFormDataModel addressFormDataModel)
        {
            OrdersDB orderDb = new OrdersDB();
            OrderModel order = orderDb.GetOne(addressFormDataModel.OrderId);

            if (order.Status == "succesful")
            {
                carts[order.UserId].Clear();
                return View("ThankYou");
            }

            AddressDb addressDb = new AddressDb();
            addressDb.Create(addressFormDataModel);

            order.Status = "succesful";
            order.TimePurchased = DateTime.UtcNow;
            order.AddressId = AddressDb.lastId;

            orderDb.Update(order);
            carts[order.UserId].Clear();

            return View("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return View();
        }


        public IActionResult Add(int id)
        {
            string? email = HttpContext.Session.GetString("Email");

            if (email == null)
            {
                TempData["status"] = "should-be-logged";
                return Redirect("/login");
            }

            int userId = new UsersDB().GetId(email);

            if (!CartController.carts.ContainsKey(userId))
                carts.Add(userId, new CartModel());

            if (userId != -1)
                CartController.carts[userId].Add(id);

            HttpContext.Session.SetString("isEmptyCart", "false");

            return Redirect(Request.Headers.Referer.ToString());
        }

        public void Remove(int i)
        {
            string? email = HttpContext.Session.GetString("Email");

            if (email == null)
                return;

            int userId = new UsersDB().GetId(email);

            if (userId == -1)
                return;

            carts[userId].Remove(i);
        }
    }
}
