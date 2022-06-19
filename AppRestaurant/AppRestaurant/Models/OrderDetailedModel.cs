namespace AppRestaurant.Models
{
    public class OrderDetailedModel
    {
        public OrderDetailedModel(int id, string userEmail, string dishesAsString, string status, decimal cost)
        {
            Id = id;
            UserEmail = userEmail;
            DishesAsString = dishesAsString;
            Status = status;
            Cost = cost;
        }

        public OrderDetailedModel(int id, string addressStreet, string userPhone, string userEmail, string dishesAsString, DateTime timePurchased, string status, decimal cost)
        {
            Id = id;
            AddressStreet = addressStreet;
            UserPhone = userPhone;
            UserEmail = userEmail;
            DishesAsString = dishesAsString;
            TimePurchased = timePurchased;
            Status = status;
            Cost = cost;
        }

        public int Id { get; set; }
        public string AddressStreet { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string DishesAsString { get; set; }
        public DateTime? TimePurchased { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
    }
}
