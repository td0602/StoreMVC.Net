
using Microsoft.AspNetCore.Mvc;
using Store_App.Models;

namespace StoreApp.Controllers;

public class AccessController : Controller
{
    private readonly AppDbContext _dbContext;
    public AccessController(AppDbContext dcContext) {
        _dbContext = dcContext;
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login() {
        if(HttpContext.Session.GetString("UserName") == null) {
            return View();
        } else {
            TempData["SuccessMessage"] = "Bạn đã đăng nhập";
            return RedirectToAction("Index", "Home");
        } 
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(User user) {
        // ktra nếu chưa đăng nhập thì nạp tk này vào
        if(HttpContext.Session.GetString("UserName") == null) {
            // kiểm tra xem user có trong CSDL không?
            var u = _dbContext.Users.Where(item => item.UserName.Equals(user.UserName) && 
            item.Password.Equals(user.Password)).FirstOrDefault();
            if(u != null) {
                // sau khi đăng nhập thì nạp tài khoản đó vào Session
                HttpContext.Session.SetString("UserName", u.UserName.ToString());
                TempData["SuccessMessage"] = "Bạn đã đăng nhập thành công: Username = " + u.UserName;
                return RedirectToAction("Index", "Home");
            } else {
                TempData["ErrorMessage"] = "Đăng nhập thất bại";
                return View(user);
            }
        }
        TempData["SuccessMessage"] = "Bạn đã đăng nhập";
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout() {
        HttpContext.Session.Clear();
        HttpContext.Session.Remove("UserName");
        return RedirectToAction("Login", "Access");
    }
}