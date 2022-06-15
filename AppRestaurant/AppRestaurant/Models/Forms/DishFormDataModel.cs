using AppRestaurant.Utils;

namespace AppRestaurant.Models.Forms
{
    public class DishFormDataModel
    {
        private string name;
        private string imageUrl;
        private string description;

        public int Grams { get; set; }
        public decimal Price { get; set; }

        public int Id { get; set; }

        public string Name
        {
            get { return name; }
            set 
            {
                if (value == null)
                    return;

                name = EscapeUnsafeChars.Escape(value);
            }
        }
        public string ImageUrl 
        {
            get { return imageUrl; }
            set 
            {
                if (value == null)
                    return;

                imageUrl = EscapeUnsafeChars.Escape(value); 
            } 
        }
        public string Description {
            get { return description; }
            set 
            {
                if (value == null)
                    return;

                description = EscapeUnsafeChars.Escape(value); 
            }
        }
    }
}
