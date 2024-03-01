
using Microsoft.AspNetCore.Mvc;
using Store_App.Models;

namespace Store_App.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin")]
public class AdminHomeController : Controller
{
    private readonly AppDbContext _dbContext;
    public AdminHomeController(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    [Route("home")]
    public IActionResult Index() {
        return View();
    }
}