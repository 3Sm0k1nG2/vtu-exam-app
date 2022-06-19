namespace AppRestaurant.Models
{
    public class MessageModel
    {
        static string[] StatusCodes = {
            "primary",
            "secondary",
            "success",
            "danger",
            "warning",
            "info",
            "light",
            "dark"
            };
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public MessageModel(string message)
        {
            Message = message;
        }
        public MessageModel(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public string AlertCode
        {
            get
            {
                string alertCode = "info";

                if (this.StatusCode > 0 && this.StatusCode < MessageModel.StatusCodes.Length)
                    alertCode = MessageModel.StatusCodes[this.StatusCode];


                return alertCode;
            }
        }
    }
}
