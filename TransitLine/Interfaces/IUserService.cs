using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransitLine.Models;
using System.Threading.Tasks;

namespace TransitLine.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> RegisterUser(User user, string role = "User");
        IActionResult LoginUser(User user, HttpContext httpContext);
        User GetUserById(int userId);
        User GetRequestingUser(string requestingUserId);
        bool IsAdminOrManager(User user);
      

    }
}
