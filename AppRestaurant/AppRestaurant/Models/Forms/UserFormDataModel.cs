using AppRestaurant.Utils;

namespace AppRestaurant.Models.Forms
{
    public class UserFormDataModel
    {

        public class LoginModel
        {
            private string email;
            public string Email
            {
                get { return email; }
                set { email = EscapeUnsafeChars.Escape(value); }
            }
            public string Password { get; set; }

            public bool IsVerified()
            {
                if (Email == null || Password == null)
                {
                    return false;
                }

                return true;
            }
        }

        public class RegModel
        {
            private string nickname;
            private string email;
            public string Nickname
            {
                get { return nickname; }
                set 
                {
                    if (value == null)
                        return;

                    nickname = EscapeUnsafeChars.Escape(value); 
                }
            }
            public string Email
            {
                get { return email; }
                set 
                {
                    if (value == null)
                        return;

                    email = EscapeUnsafeChars.Escape(value); 
                }
            }
            public string Password { get; set; }
            public string Repass { get; set; }

            public bool IsVerified()
            {
                if (Email == null || Password == null)
                {
                    return false;
                }

                if (Password.Length < 8)
                {
                    return false;
                }

                if (Password == Repass)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
