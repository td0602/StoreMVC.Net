using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Store_App.Models;

namespace Store_App.Areas.User.Controllers;

[Area("User")]
[Route("orders")]
public class OrderController : Controller
{
    private readonly AppDbContext _dbContext;
    public OrderController(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index() {
        return View();
    }
}