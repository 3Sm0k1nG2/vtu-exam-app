namespace AppRestaurant.Models.User
{
    public class DishUserModel
    {
        readonly private int _id;

        public DishUserModel(DishModel dishModel)
        {
            _id = dishModel.Id;
            Name = dishModel.Name;
            Grams = dishModel.Grams;
            Price = dishModel.Price;
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Name { get; set; }
        public int Grams { get; set; }
        public decimal Price { get; set; }
    }
}
