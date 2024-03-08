using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_App.Models.DTO;
using Store_App.Repositories;

namespace Store_App.Controllers;

public class UserAuthenticationController : Controller
{
    private readonly IUserAuthenticationService _service;
    public UserAuthenticationController (IUserAuthenticationService service) {
        _service = service;
    }

    public IActionResult Registration() {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Registration(RegistrationModel model) {
        if(!ModelState.IsValid) {
            return View(model);
        }
        model.Role = "user";
        var result = await _service.RegisterAsync(model);
        TempData["msg"] = result.Message;
        return RedirectToAction(nameof(Registration));
    }

    [Route("/login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login (LoginModel model) {
        if(!ModelState.IsValid) {
            return View(model);
        }

        var result = await _service.LoginAsync(model);
        // User.IsInRole("admin") 
        // var isAdminClaim = User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "admin");
        // 2 cach tren chua kịp cập nhật nên sd dòng dưới
        if(result.StatusCode == 1) {
            if(result.isAdmin){
                return RedirectToAction("Index", "AdminHome", new {area = "Admin"});
            } else {
                return RedirectToAction("Index", "Home");
            }
        } else {
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Login));
        }
    }

    [Authorize]
    public async Task<IActionResult> Logout() {
        await _service.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> RegisterAdmin() {
        var model = new RegistrationModel {
            Username = "admin",
            Name = "Doan",
            Address = "Bắc Giang",
            Email = "doan@gmail.com",
            Password = "admin",
            Role = "admin",
            PhoneNumber = "025444848"
        };
        var result = await _service.RegisterAsync(model);
        return Ok(result);
    }

}