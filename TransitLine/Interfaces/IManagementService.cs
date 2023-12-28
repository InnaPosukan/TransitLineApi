using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TransitLine.DTO;
using TransitLine.Models;

public interface IManagementService
{
    Task<IActionResult> AddUser(User user, ClaimsPrincipal userClaims);

    Task<IActionResult> AssignDriverToOrder(AssignDriverDTO request);
    Task<IActionResult> RejectOrder(Order order, ClaimsPrincipal userClaims);

    IActionResult GetAllUsers();
    IActionResult GetAllDriversWithTrucks();
    IActionResult GetAllDeliveries();


    Task<IActionResult> UpdateUser(int userId, User updatedUser);
    Task<IActionResult> DeleteUser(int userId);

}
