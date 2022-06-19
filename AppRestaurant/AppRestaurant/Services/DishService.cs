using AppRestaurant.Data;
using AppRestaurant.Models;
using AppRestaurant.Models.Forms;

namespace AppRestaurant.Services
{
    public class DishService
    {
        public void Create(DishFormDataModel formModel)
        {
            new DishDb().Create(new DishModel(formModel));
        }

        public string? GetOneName(int id)
        {
            return new DishDb().GetOneName(id);
        }

        public DishModel? GetOne(int id)
        {
            return new DishDb().GetOne(id);
        }
        public List<DishModel> GetAll()
        {
            return new DishDb().GetAll();
        }
        public List<DishModel> GetAll(string query)
        {
            return new DishDb().GetAll(query);
        }

        public void Update(int id, DishFormDataModel newModel)
        {
            new DishDb().Update(id, new DishModel(newModel));
        }

        public void Delete(int id)
        {
            new DishDb().Delete(id);
        }
    }
}
