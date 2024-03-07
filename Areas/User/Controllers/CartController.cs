using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_App.Models;
using Store_App.Models.CustomIdentity;

namespace Store_App.Areas.User.Controllers;
[Area("User")]
[Authorize(Roles ="user")]
public class CartController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public CartController(AppDbContext dbContext, UserManager<AppUser> userManager) {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [Route("/list-carts")]
    public async Task<IActionResult> Index() {
        var carts = await _dbContext.Carts.ToListAsync();

        return View(carts);
    }

    [Route("/add-cart")]
    public async Task<IActionResult> AddCart(int BookId, int Quantity) {
        var book = await _dbContext.Books.FindAsync(BookId);
        var cartCheck = await _dbContext.Carts.Where(c => c.BookId == BookId && c.AppUserId == Convert.ToInt32(_userManager.GetUserId(User))).FirstOrDefaultAsync();
        if(book == null) {
            TempData["ErrorMessage"] = "Không thể thêm sản phẩm vào giỏ hàng";
        } else if(cartCheck == null) {
            var cart = new Cart {
                BookName = book.BookName,
                Quantity = Quantity,
                Image = book.Image,
                Price = book.Price,
                Total = Quantity * book.Price,
                Status = true,
                AppUserId = Convert.ToInt32(_userManager.GetUserId(User)),
                BookId = book.BookId
            };
            _dbContext.Carts.Add(cart);
            await _dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công";
        } else {
            cartCheck.Quantity += Quantity;
            cartCheck.Total += Quantity*cartCheck.Price;
            _dbContext.Carts.Update(cartCheck);
            await _dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công";
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost("/update-cart")]
    // [Route("/update-cart")]
    public async Task UpdateCart(int CartId, int NewQuantity) {
        Console.WriteLine($"===========================================CartId= {CartId} NewQuantity= {NewQuantity}");
        var cart = await _dbContext.Carts.FindAsync(CartId);
        if(cart == null) {
            TempData["ErrorMessage"] = "Không thể cập nhật giỏ hàng";
        } else {
            cart.Quantity = NewQuantity;
            cart.Total = cart.Price*cart.Quantity;
            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync();
        }
    }

    [Route("/remove-cart")]
    public async Task<IActionResult> RemoveCart(int CartId) {
        var cart = await _dbContext.Carts.FindAsync(CartId);
        if(cart == null) {
            return NotFound();
        } else {
            _dbContext.Carts.Remove(cart);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Cart", new {area = "User"});
        }
    }
}