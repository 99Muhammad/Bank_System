using System.Text;

namespace BankSystemProject.Helpers
{
    public static class Base64Helper
    {
        // Method to encode a string to Base64
        public static string Encode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);  // Convert input string to bytes
            return Convert.ToBase64String(bytes);  // Convert bytes to Base64 string
        }

        // Method to decode a Base64 string back to the original string
        public static string Decode(string encodedInput)
        {
            var bytes = Convert.FromBase64String(encodedInput);  // Convert Base64 string back to bytes
            return Encoding.UTF8.GetString(bytes);  // Convert bytes back to original string
        }
    }
}
