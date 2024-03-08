using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_App.Models;
using Store_App.Models.CustomIdentity;
using Store_App.Models.DTO;
using Store_App.Models.ViewModels;

namespace Store_App.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles ="user")]
public class OrderController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    public OrderController(AppDbContext dbContext, UserManager<AppUser> userManager) {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [Route("list-orders")]
    public async Task<IActionResult> Index() {
        var user = await _userManager.GetUserAsync(User);
        var PhoneNumber = await _userManager.GetPhoneNumberAsync(user);
        var orders = await _dbContext.Orders.Where(o => o.PhoneNumber == PhoneNumber).ToListAsync();
        return View(orders);
    }

    [HttpGet("/add-order")]
    public async Task<IActionResult> AddOrder() {
        var user = await _userManager.GetUserAsync(User);
        var orderViewModel = new OrderViewModel();
        orderViewModel.User = user;

        orderViewModel.Carts = await _dbContext.Carts.Where(c => c.AppUserId == Convert.ToInt32(_userManager.GetUserId(User)) && c.Status==true).ToListAsync();
        
        return View(orderViewModel);
    }

    [HttpPost("/add-order")]
    public async Task<IActionResult> AddOrder( OrderViewModel model) {
        var carts = await _dbContext.Carts.Where(c => c.AppUserId == Convert.ToInt32(_userManager.GetUserId(User)) && c.Status == true).ToListAsync();
        var order = new Order {
            OrderCode = _userManager.GetUserName(User) + "-" + Guid.NewGuid().ToString(),
            FullName = model.OrderView.FullName,
            Address = model.OrderView.Address,
            PhoneNumber = model.OrderView.PhoneNumber,
            Email = model.OrderView.Email,
            OrderTotal = carts.Sum(c => c.Total) + 50,
            Payment = model.OrderView.PayMent,
            Note = model.OrderView.Note,
            DateCreated = DateTime.Now
        };
        foreach(var item in carts) {
            item.Status = false;
            item.order = order;
        }
        _dbContext.Carts.UpdateRange(carts);
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đặt hàng thành công";
        return RedirectToAction("Index", "Home");
    }

    [Route("/order-detail")]
    public async Task<IActionResult> OrderDetail(int OrderId) {
        var order = await _dbContext.Orders.Include(o => o.carts).Where(o => o.OrderId == OrderId).FirstOrDefaultAsync();
        if(order == null) {
            return NotFound();
        }

        return View(order);
    }

    [Route("cancel-order")]
    public async Task<IActionResult> CancelOrder(int OrderId) {
        var order = await _dbContext.Orders.FindAsync(OrderId);
        if(order == null) {
            return NotFound();
        }
        var carts = await _dbContext.Carts.Where(c => c.OrderId == OrderId).ToListAsync();
        _dbContext.Carts.RemoveRange(carts);
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Hủy đơn hàng thành công";
        return RedirectToAction("Index", "Home");
    }
}