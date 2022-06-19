using AppRestaurant.Models;
using AppRestaurant.Models.Forms;
using AppRestaurant.Data;

namespace AppRestaurant.Services
{
    public class AddressService
    {
        public int GetLastID()
        {
            return AddressDb.lastId;
        }
        public void Create(AddressFormDataModel formModel)
        {
            new AddressDb().Create(formModel);
        }
    }
}
