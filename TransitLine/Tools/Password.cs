using System.Security.Cryptography;
using System.Text;

namespace TransitLine.Tools
{
    public class Password
    {
        public static string hashPassword(string password)
        {
            var sha = SHA256.Create();  
            var asByteArray = Encoding.UTF8.GetBytes(password); 
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }
    }
}
