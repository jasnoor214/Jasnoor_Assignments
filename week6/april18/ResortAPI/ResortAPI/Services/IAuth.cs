using Microsoft.AspNetCore.Mvc;
using ResortAPI.Models;

namespace ResortAPI.Services
{
    public interface IAuth
    {
        Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role);
        Task<IActionResult> Login([FromBody] LoginModel loginModel);
    }
}
