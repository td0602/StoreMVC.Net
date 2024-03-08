using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_App.Models;
using X.PagedList;

namespace Store_App.Areas.Admin.Controllers;
[Authorize(Roles = "admin")]
[Area("Admin")]
public class OrderController : Controller
{
    private readonly AppDbContext _dbContext;

    public OrderController(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    [Route("/admin/list-orders")]
    public async Task<IActionResult> Index(int? page, string? searchString, string currentFilter) {
        
        if(searchString != null) {
            page = 1;
        } else {
            searchString = currentFilter;
        }
        ViewBag.currentFilter = searchString;
        var orders = from order in _dbContext.Orders select order;
        if(searchString != null) {
            orders = from order in orders where order.FullName.Contains(searchString) select order;
        }

        int pageNumber = (page == null || page < 1) ? 1 : page.Value;
        int pageSize = 4;
        PagedList<Order> lst = new PagedList<Order>(await orders.ToListAsync(), pageNumber, pageSize);

        return View(lst);
    }

    [Route("/admin/delete-order")]
    public async Task<IActionResult> DeleteOrder(int OrderId) {
        var order =  await _dbContext.Orders.FindAsync(OrderId);
        if(order == null) {

            return NotFound();
        }

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa đơn hàng thành công";
        return RedirectToAction(nameof(Index));
    }

    [Route("/admin/order-detail")]
    public async Task<IActionResult> OrderDetail(int OrderId) {
        var order = _dbContext.Orders.Include(o => o.carts).Where(o => o.OrderId == OrderId).FirstOrDefault();
        if(order == null) {
            return NotFound();
        }
        return View(order);
    }
}