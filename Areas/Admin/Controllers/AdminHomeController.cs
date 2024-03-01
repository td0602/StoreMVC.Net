
using Microsoft.AspNetCore.Mvc;
using Store_App.Models;

namespace Store_App.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin/[action]")]
public class AdminHomeController : Controller
{
    private readonly AppDbContext _dbContext;
    public AdminHomeController(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    public IActionResult Index() {
        return View();
    }
}