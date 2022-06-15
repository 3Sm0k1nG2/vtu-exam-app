namespace AppRestaurant.Models
{
    public class CartModel
    {
        private Dictionary<int, int> dishesIdsCount;

        public CartModel()
        {
            this.dishesIdsCount = new Dictionary<int, int>();
        }

        public void Add(int id) 
        { 
            if(!dishesIdsCount.ContainsKey(id))
                dishesIdsCount.Add(id, 0);

            dishesIdsCount[id]++;
        }
        public void Remove(int id) 
        {
            if (!dishesIdsCount.ContainsKey(id))
                return;

            if (dishesIdsCount[id] == 0)
                return;

            dishesIdsCount[id]--;
        }
        public void Clear() { dishesIdsCount.Clear(); }
        public Dictionary<int, int> GetDishesIdsAndCount { get { return dishesIdsCount; } }
        public List<int> GetDishesIds { get { return dishesIdsCount.Keys.ToList(); } }

    }
}
