namespace AppRestaurant.Models
{
    public class OrderModel
    {
        // To Implement - DB Model

        public OrderModel()
        {

        }

        public OrderModel(int id, int userId, string dishesIdsString, string status, decimal cost)
        {
            Id = id;
            DishesIdsString = dishesIdsString;
            UserId = userId;
            Status = status;
            Cost = cost;
        }

        public OrderModel(int id, int userId, int addressId, string dishesIdsString, DateTime timePurchased, string status, decimal cost)
        {
            Id = id;
            AddressId = addressId;
            UserId = userId;
            DishesIdsString = dishesIdsString;
            TimePurchased = timePurchased;
            Status = status;
            Cost = cost;
        }

        public int Id { get; set; }
        public int? AddressId { get; set; } = null;
        public int UserId { get; set; }
        public string DishesIdsString { get; set; }
        public List<int> DishesIds { get; set; }
        public DateTime? TimePurchased { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
    }
}
