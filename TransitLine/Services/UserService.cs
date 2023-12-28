using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TransitLine.DBContext;
using TransitLine.Models;
using TransitLine.Tools;
using TransitLine.Interfaces;
using System.Net;

namespace TransitLine.Services
{
    public class UserService : IUserService
    {
        private readonly TransitLineContext _context;
        private readonly IConfiguration _configuration;

        public UserService(TransitLineContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IActionResult> RegisterUser(User user, string role = "User")
        {
            try
            {
                var dbUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

                if (dbUser != null)
                {
                    return new BadRequestObjectResult("User with this email already exists");
                }

                user.Password = Password.hashPassword(user.Password);
                user.Role = role;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new OkObjectResult("User is successfully registered");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public IActionResult LoginUser(User user, HttpContext httpContext)
        {
            try
            {
                string password = Password.hashPassword(user.Password);
                var dbUser = _context.Users
                    .Where(u => u.Email == user.Email && u.Password == password)
                    .Select(u => new { u.IdUser, u.Email, u.Role })
                    .FirstOrDefault();

                if (dbUser == null)
                {
                    return new BadRequestObjectResult("Username or password is incorrect");
                }

                if (httpContext.Items["User"] is JwtSecurityToken existingToken)
                {
                    return new BadRequestObjectResult("User is already logged in");
                }

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, dbUser.Email),
                    new Claim("userID", dbUser.IdUser.ToString()),
                    new Claim(ClaimTypes.Role, dbUser.Role),
                    new Claim("email", dbUser.Email)
                };

                var token = GetToken(authClaims);
                httpContext.Items["User"] = token;

                return new OkObjectResult(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public User GetRequestingUser(string requestingUserId)
        {
            if (int.TryParse(requestingUserId, out var userId))
            {
                return GetUserById(userId);
            }

            return null;
        }
       
        public bool IsAdminOrManager(User user)
        {
            return user != null && (user.Role == "Admin" || user.Role == "Manager");
        }

        private JwtSecurityToken GetToken(List<Claim> authClaim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
