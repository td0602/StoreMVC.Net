using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Store_App.Controllers;

[Authorize] // Yêu cầu đăng nhập
public class DashboardController : Controller
{
    public IActionResult Display() {
        return View();
    }
}