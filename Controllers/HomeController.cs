using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store_App.Models;
using X.PagedList;

namespace Store_App.Controllers;

// [Route("home")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task<IActionResult> Index()
    {
        var books = await _dbContext.Books.Include(b => b.Category).ToListAsync();
        return View(books);
    }

    [Route("book-detail")]
    public async Task<IActionResult> Detail(int BookId) {
        if(BookId == null || _dbContext.Books == null) {
            return NotFound();
        }
        var books = from b in _dbContext.Books.Include(b => b.Category) where b.BookId == BookId select b;
        var book = books.FirstOrDefault();
        return View(book);
    }

    [Route("grid-shop")]
    public async Task<IActionResult> GridShop(int? page, int? CategoryId, string? SearchString, string? CurrentFilter, string? OrderString) 
    {
        if(SearchString != null) {
            page = 1;
        } else {
            SearchString = CurrentFilter;
        }
        ViewBag.categoryId = CategoryId;
        ViewBag.currentFilter = SearchString;
        ViewBag.orderString = OrderString;

        var books = from b in _dbContext.Books.Include(b => b.Category) select b;

        if(CategoryId != null) {
            books = from b in books where b.CategoryId == CategoryId select b;
        }
        if(OrderString != null) {
            if(OrderString.Equals("ASC")) {
                books = books.OrderBy(b => b.Price);
            }
            if(OrderString.Equals("DESC")) {
                books = books.OrderByDescending(b => b.Price);
            }
        }
        if(SearchString != null) {
            books = books.Where(b => b.BookName.Contains(SearchString));
        }

        int pageNumber = (page == null || page < 1) ? 1 : page.Value;
        int pageSize = 4;
        PagedList<Book> lst = new PagedList<Book>(books, pageNumber, pageSize);
        return View(lst);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Contact() {
        return View();
    }
}
