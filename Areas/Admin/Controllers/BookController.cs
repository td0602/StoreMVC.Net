
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store_App.Models;
using Store_App.Services;
using X.PagedList;

namespace Store_App.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin/book")]
public class BookController : Controller
{
    private readonly FileUploadService _fileUpload;
    private readonly AppDbContext _dbContext;
    public BookController(AppDbContext dbContext, FileUploadService fileUpload) {
        _dbContext = dbContext;
        _fileUpload = fileUpload;
    }

    [Route("list-books")]
    public async Task<IActionResult> Index(string? searchString, string? currentFilter, int? page) {
        if(searchString != null) {
            page = 1;
        } else {
            searchString = currentFilter;
        }
        ViewBag.currentFilter = searchString;

        var books = from b in _dbContext.Books.Include(b => b.Category) select b;
        var categorys = _dbContext.Categories;
        if(!String.IsNullOrEmpty(searchString)) {
            books = books.Where(b => b.BookName.Contains(searchString)); // lấy thêm cả property Category cho book
        }

        int pageNumber = (page == null || page < 1) ? 1 : page.Value;
        int pageSize = 2;
        PagedList<Book> lst = new PagedList<Book>(books, pageNumber, pageSize);
        return View(lst);
    }

    [Route("create-book")]
    public async Task<IActionResult> Create() {
        //Lay ra list Category để select
        ViewBag.CategoryId = new SelectList(await _dbContext.Categories.ToListAsync(), "CategoryId", "CategoryName");
        return View();
    }
    [HttpPost]
    [Route("create-book")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book, IFormFile? fileImage) {
        if(ModelState.IsValid) {
            var fileName = await _fileUpload.UploadFileAsync(fileImage);
            book.Image = fileName;
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm mới sản phẩm thành công";
            return RedirectToAction("Index");
        }

        // xuất lỗi nếu ModelState.Values == false
        foreach (var entry in ModelState.Values)
            {
                foreach (var error in entry.Errors)
                {
                    var errorMessage = error.ErrorMessage;
                    Console.WriteLine($"errorMessage==== " + errorMessage);
                }
            }

        ViewBag.CategoryId = new SelectList(await _dbContext.Categories.ToArrayAsync(), "CategoryId", "CategoryName", book.CategoryId);
        return View(book);
    }

    [Route("edit-book")]
    public async Task<IActionResult> Edit(int BookId) {
        if(BookId == null || _dbContext.Books == null) {
            return NotFound();
        }
        var book = await _dbContext.Books.FindAsync(BookId);
        if(book == null) {
            return NotFound();
        }
        ViewBag.CategoryId = new SelectList(_dbContext.Categories, "CategoryId", "CategoryName", book.CategoryId);
        return View(book);
    }
    [HttpPost]
    [Route("edit-book")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Book book, IFormFile? fileImage) {
        if(ModelState.IsValid) {
            book.Image = await _fileUpload.UploadFileAsync(fileImage);
            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công";
            return RedirectToAction("Index");
        }
        ViewBag.CategoryId = new SelectList(_dbContext.Categories, "CategoryId", "CategoryName", book.CategoryId);
        return View(book);
    }

    public async Task<IActionResult> Delete(int? BookId) {
        if(BookId == null || _dbContext.Books == null) {
            TempData["ErrorMessage"] = "Sản phẩm không tồn tại";
            return NotFound();
        }
        var book = await _dbContext.Books.FindAsync(BookId);
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa sản phẩm thành công";
        return RedirectToAction("Index");
    }
}