using Microsoft.AspNetCore.Mvc;
using TransitLine.Models;
using TransitLine.Interfaces;


namespace TransitLine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> UserRegistration([FromBody] User user, string role = "User")
        {
            return await _userService.RegisterUser(user, role);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult UserLogin([FromBody] User user)
        {
            return _userService.LoginUser(user, HttpContext);
        }
      

    }
}
