using AppRestaurant.Data;
using AppRestaurant.Models;
using System.Text;

namespace AppRestaurant.Services
{
    public class OrderService
    {
        public int GetLastId()
        {
            return OrderDb.lastId;
        }

        public void Create(int userId, decimal finalCost, Dictionary<int, int> dishesIdsAndCount)
        {
            StringBuilder dishes = new StringBuilder();

            // Quantity x Id
            foreach (var dish in dishesIdsAndCount)
                dishes.Append($"{dish.Value}x{dish.Key},");

            dishes.Remove(dishes.Length - 1, 1);

            OrderModel model = new OrderModel(userId, dishes.ToString(), "pending", finalCost);
            new OrderDb().Create(model);
        }
        public OrderModel GetOne(int id)
        {
            return new OrderDb().GetOne(id);
        }

        public void PatchAddressFinal(int orderId, int addressId)
        {
            new OrderDb().PatchAddressFinal(new OrderModel(orderId, addressId, DateTime.UtcNow, "successful"));
        }

        public List<OrderDetailedModel> GetAllDetailed(int? userId = null)
        {
            OrderDb ordersDb = new OrderDb();
            List<OrderDetailedModel>? orderDetailedModels = ordersDb.GetAllDetailed(userId);

            foreach (var model in orderDetailedModels)
                model.DishesAsString = MapDishes(model.DishesAsString);

            return orderDetailedModels;
        }

        public OrderDetailedModel GetOneDetailed(int id)
        {
            OrderDb ordersDb = new OrderDb();
            OrderDetailedModel model = ordersDb.GetOneDetailed(id);

            model.DishesAsString = MapDishes(model.DishesAsString);

            return model;
        }

        public void DeleteOne(int id)
        {
            OrderDb ordersDb = new OrderDb();
            ordersDb.DeleteOne(id);
        }

        private string MapDishes(string dishesRaw)
        {
            Dictionary<int, string> dishNames = new Dictionary<int, string>();
            string[] dishes = dishesRaw.Split(',');

            for (int i = 0; i < dishes.Length; i++)
            {
                string[] dishSplitted = dishes[i].Split('x');

                int dishId = int.Parse(dishSplitted[1]);
                string? dishName;
                try
                {
                    dishName = dishNames[dishId];
                }
                catch (KeyNotFoundException)
                {
                    dishName = new DishService().GetOneName(dishId);
                    dishNames.Add(dishId, dishName);
                }

                if (dishName != null)
                {
                    dishes[i] = $"{dishSplitted[0]}x {dishName}";
                }
            }
            return String.Join(',', dishes);
        }
    }
}
