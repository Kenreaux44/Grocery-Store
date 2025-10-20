namespace MyfirstLib.Models
{

    public class Tools
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public static bool Toggle { get; } = false;

        public static string GetMessage()
        {
            return "Anything I want!";
        }
    }
}
