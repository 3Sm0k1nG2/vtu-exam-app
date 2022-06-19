using AppRestaurant.Utils;
using AppRestaurant.Models.Forms;

namespace AppRestaurant.Models
{
    public class UserModel
    {
        private string password;
        protected bool isAuthorized;

        // Login a user
        public UserModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public UserModel(UserFormDataModel.LoginModel formModel)
        {
            Email = formModel.Email;
            Password = formModel.Password; ;
        }

        // Registering a user
        public UserModel(string nickname, string email, string password)
        {
            Nickname = nickname;
            Email = email;
            Password = password;
        }

        public UserModel(UserFormDataModel.RegModel formModel)
        {
            Nickname = formModel.Nickname;
            Email = formModel.Email;
            Password = formModel.Password;
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
