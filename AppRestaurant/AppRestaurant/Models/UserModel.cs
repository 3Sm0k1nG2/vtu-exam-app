using AppRestaurant.Utils;
using AppRestaurant.Models.Forms;

namespace AppRestaurant.Models
{
    public class UserModel
    {
        private string password;
        protected bool isAuthorized;

        // Registering a user
        public UserModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public UserModel(string nickname, string email, string password)
        {
            Nickname = nickname;
            Email = email;
            Password = password;
        }

        // Returning user from DB
        public UserModel(int id, string email)
        {
            Id = id;
            Email = email;
        }

        public UserModel(int id, string nickname, string email)
        {
            Id = id;
            Nickname = nickname;
            Email = email;
        }

        public UserModel(UserFormDataModel.LoginModel model)
        {
            Email = model.Email;
            Password = model.Password;
        }

        public UserModel(UserFormDataModel.RegModel model)
        {
            Nickname = model.Nickname;
            Email = model.Email;
            Password = model.Password;
        }

        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = MD5_Algorithm.CalculateMD5Hash(value);
            }
        }
    }
}
