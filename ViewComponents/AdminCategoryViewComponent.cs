using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_App.Models;

namespace Store_App.ViewComponents;

public class AdminCategoryViewComponent : ViewComponent
{
    private readonly AppDbContext _dbContext;
    public AdminCategoryViewComponent(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<IViewComponentResult> InvokeAsync() {
        var categories = await _dbContext.Categories.OrderBy(c => c.CategoryName).ToListAsync();
        return View(categories);
    }
}