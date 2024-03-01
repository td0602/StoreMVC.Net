using Store_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace StoreApp.Areas.Admin.Controllers;
[Area("Admin")]
[Route("admin/[controller]")]
public class CategoryController : Controller
{
    private readonly AppDbContext _dbContext;
    public CategoryController(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    [Route("list-categorys")]
    public async Task<IActionResult> Index(string currentFilter, string searchString, int? page) {
        if(searchString != null) {
            page = 1;
        } else {
            searchString = currentFilter;
        }
        // lưu lại chuỗi filter cho currentFilter lần lọc tới
        ViewBag.currentFilter = searchString;

        var categorys = from c in _dbContext.Categories select c;

        if(!String.IsNullOrEmpty(searchString)) {
            categorys = categorys.Where(c => c.CategoryName.Contains(searchString));
        }

        int pageSize = 2;
        int pageNumber = (page == null || page < 1) ? 1 : page.Value;
        PagedList<Category> lst = new PagedList<Category>(categorys, pageNumber, pageSize);
        return View(lst);
    }

    [HttpGet]
    [Route("create-category")]
    public IActionResult Create() { // admin/category/create-category
        return View();
    }
    [HttpPost]
    [Route("create-category")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category) { // admin/category/create-category
        if(ModelState.IsValid) {
            _dbContext.Categories.Add(category);
           await _dbContext.SaveChangesAsync();
           TempData["SuccessMessage"] = "Thêm mới thể loại thành công!";
            return RedirectToAction("Index");
        }
        return View(category);
    }

    // [Route("Delete")]
    public async Task<IActionResult> Delete(int CategoryId) {
        if(CategoryId == null || _dbContext.Categories == null) {
            TempData["ErrorMessage"] = "Không tồn tại thể loại sách để xóa!";
            return NotFound(); // Error 404
        }
        var category = await _dbContext.Categories.FirstOrDefaultAsync(item => item.CategoryId == CategoryId);
        if(category == null) {
            return NotFound(); // Error 404
        }
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa thể loại thành công!";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("edit-category")]
    public async Task<IActionResult> Edit(int CategoryId) {
        Console.WriteLine("CategoryId= " + CategoryId);
        if(CategoryId == null || _dbContext.Categories == null) {
            return NotFound();
        }
        var category = await _dbContext.Categories.FindAsync(CategoryId);
        if(category == null) {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    [Route("edit-category")]
    [ValidateAntiForgeryToken] // validate data
    public async Task<IActionResult> Edit(Category category) {
        if(ModelState.IsValid) {
            // sử dụng try catch vì Các Entity trong CSDL có thể bị ràng buộc lẫn nhau --> có thể k update dc
            try{
                _dbContext.Categories.Update(category);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thể loại thành công";
                return RedirectToAction("Index");
            } catch (DbUpdateConcurrencyException e) {
                TempData["ErrorMessage"] = "Không thể cập nhật thể loại";
                Console.WriteLine(e.Message);
            }
        }
        return View(category);
    }

    // [HttpGet]
    // public async Task<IActionResult> Search(string? Keyword, int? page) {
    //     int pageNumber = (page ==null || page < 1) ? 1 : page.Value;
    //     int pageSize = 2;
    //     var listCategories = _dbContext.Categories.
    //     return View()
    // }
}