using AppRestaurant.Models.Forms;

namespace AppRestaurant.Models
{
    public class DishModel
    {
        readonly private int _id;

        public DishModel() { }
        public DishModel(int id, string name, int grams, decimal price, string description, string imgUrl)
        {
            _id = id;
            Name = name;
            Grams = grams;
            Price = price;
            Description = description;
            ImageUrl = imgUrl;
        }
        public DishModel(string name, int grams, decimal price, string description, string imgUrl)
        {
            Name = name;
            Grams = grams;
            Price = price;
            Description = description;
            ImageUrl = imgUrl;
        }

        public DishModel(DishFormDataModel model)
        {
            Name = model.Name;
            Grams = model.Grams;
            Price = model.Price;
            Description = model.Description;
            ImageUrl = model.ImageUrl;
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
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
