namespace AppRestaurant.Models.Forms
{
    public class OrderFormDataModel
    {
        // To implement
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string DishesIdsString { get; set; }
        public List<int> DishesIds { get; set; }
        public DateTime TimePurchased { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
    }
}
