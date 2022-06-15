namespace AppRestaurant.Utils
{
    public class EscapeUnsafeChars
    {
        static public string Escape(string unsafeText)
        {
            return unsafeText.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&#039;");
        }
    }
}
