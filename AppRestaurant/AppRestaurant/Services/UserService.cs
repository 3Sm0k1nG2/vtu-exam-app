using AppRestaurant.Data;
using AppRestaurant.Models;
using AppRestaurant.Models.Forms;

namespace AppRestaurant.Services
{
    public class UserService
    {
        public UserModel Login(string email, string password)
        {
            return new UserDb().Login(email, password);
        }
        public UserModel Login(UserFormDataModel.LoginModel formModel)
        {
            UserModel model = new UserModel(formModel);
            return new UserDb().Login(model.Email, model.Password);
        }

        public void Create(string? nickname, string email, string password)
        {
            UserModel model;
            if (nickname == null)
                model = new UserModel(email, password);
            else
                model = new UserModel(nickname, email, password);
            
            new UserDb().Create(model);
        }

        public void Create(UserFormDataModel.RegModel formModel)
        {
            UserModel model = new UserModel(formModel);
            new UserDb().Create(model);
        }

        public bool Exists(string email)
        {
            return new UserDb().Exists(email);
        }

        public UserModel? GetOne(int userId)
        {
            return new UserDb().GetOne(userId);
        }

        public UserModel? GetOne(string email)
        {
            return new UserDb().GetOne(email);
        }

        public int GetId(string email)
        {
            return new UserDb().GetId(email);
        }

        public List<UserModel> GetAll()
        {
            return new UserDb().GetAll();
        }

        public void Delete(int userId)
        {
            new UserDb().Delete(userId);
        }
        public void Delete(string email)
        {
            new UserDb().Delete(email);
        }
    }
}
