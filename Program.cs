using Store_App.Models;
using Microsoft.EntityFrameworkCore;
using Store_App.Services;
using Microsoft.AspNetCore.Identity;
using Store_App.Repositories;
using Store_App.Models.CustomIdentity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// đăng ký dịch vụ DbCOntext
builder.Services.AddDbContext<AppDbContext>(options => {
    string connectString = builder.Configuration.GetConnectionString("AppConnectionString");
    options.UseSqlServer(connectString);
    // Tắt log SQL
    options.EnableSensitiveDataLogging(false);
});

builder.Services.AddScoped<FileUploadService>();
// đăng ký dịch vụ Identity
builder.Services.AddIdentity<AppUser, CustomIdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// Cấu hình Cho Identity
builder.Services.Configure<IdentityOptions>(options => {
    //Thiết lập pass word
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 1; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 0; // Số ký tự riêng biệt
});
// đăn ký cấu hình Cookies lưu dữ liệu
builder.Services.ConfigureApplicationCookie(op => op.LoginPath = "/UserAuthentication/Login");

builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Sử dụng pipeline xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
