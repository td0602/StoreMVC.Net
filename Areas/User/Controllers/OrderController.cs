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
        // var orders = await (from o in _dbContext.Orders join c in _dbContext.Carts.Where(c => c.OrderId == Convert.ToInt32(_userManager.GetUserId(User))) 
        //             on c.OrderId == o.OrderId select o).Distinct().ToListAsync();
        var orders = await (from cart in _dbContext.Carts where cart.AppUserId == Convert.ToInt32(_userManager.GetUserId(User)) join 
                        order in _dbContext.Orders on cart.OrderId equals order.OrderId select order).Distinct().ToListAsync();
        return View(orders);
    }

    [HttpGet("/add-order")]
    public async Task<IActionResult> AddOrder(int? BookId) {
        List<Cart> carts = new List<Cart>();
        // TH order 1 cuốn sách
        if(BookId != null) {
            ViewBag.BookId = BookId;
            var book = await _dbContext.Books.FindAsync(BookId);
            if(book == null) {
                return NotFound();
            }
            var cart = new Cart {
                BookName = book.BookName,
                Quantity = 1,
                Image = book.Image,
                Price = book.Price,
                Total = 1 * book.Price,
                Status = true,
                AppUserId = Convert.ToInt32(_userManager.GetUserId(User)),
                BookId = book.BookId
            };
            carts.Add(cart);
        } else {
            carts = await _dbContext.Carts.Where(c => c.AppUserId == Convert.ToInt32(_userManager.GetUserId(User)) && c.Status==true).ToListAsync();
        }
        
        var user = await _userManager.GetUserAsync(User);
        var model = new OrderModel{
            FullName = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Carts = carts
        };
        return View(model);
    }

    [HttpPost("/add-order")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrder(OrderModel model, int? BookId) {
            //TH order 1 cuon sach
        if(BookId != null) {
            ViewBag.BookId = BookId;
            var book = await _dbContext.Books.FindAsync(BookId);
            if(book == null) {
                return NotFound();
            }
            var cart = new Cart {
                BookName = book.BookName,
                Quantity = 1,
                Image = book.Image,
                Price = book.Price,
                Total = 1 * book.Price,
                Status = true,
                AppUserId = Convert.ToInt32(_userManager.GetUserId(User)),
                BookId = book.BookId
            };
            model.Carts = new List<Cart>{cart};
        } else {
            model.Carts = await _dbContext.Carts.Where(c => c.AppUserId == Convert.ToInt32(_userManager.GetUserId(User)) && c.Status==true).ToListAsync();
        }
        
        if(!ModelState.IsValid) {
            return View(model);
        }
        // var carts = await _dbContext.Carts.Where(c => c.AppUserId == Convert.ToInt32(_userManager.GetUserId(User)) && c.Status == true).ToListAsync();
        var order = new Order {
            OrderCode = _userManager.GetUserName(User) + "-" + Guid.NewGuid().ToString(),
            FullName = model.FullName,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            OrderTotal = model.Carts.Sum(c => c.Total) + 50,
            Payment = model.Payment,
            Note = model.Note,
            DateCreated = DateTime.Now,
            carts = model.Carts
        };
        foreach(var item in order.carts) {
            item.Status = false;
        }
        await _dbContext.Orders.AddAsync(order);
        //TH dawt 1 cuon sach khong cho vao gio hang nen k tao ra cart
        // if(BookId == null) {
        //     _dbContext.Carts.UpdateRange(model.Carts);
        // }
        await _dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đặt hàng thành công";
        return RedirectToAction("Index", "Home");
    }

    // [Route("order-single-book")]
    // public async Task<IActionResult> OrderSingleBook(int BookId) {
        
    //     var user = await _userManager.GetUserAsync(User);
    //     var model = new OrderModel{
    //         FullName = user.Name,
    //         Email = user.Email,
    //         PhoneNumber = user.PhoneNumber,
    //         Address = user.Address,
    //         Carts = carts
    //     };
    //     return View("AddOrder", model);
    // }

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