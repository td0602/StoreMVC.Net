using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store_App.Models.CustomIdentity;
using Store_App.Models.DTO;
using Store_App.Services;

namespace Store_App.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "user")]
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly FileUploadService _fileUpload;
    public AccountController(UserManager<AppUser> userManager, FileUploadService fileUpload) {
        _userManager = userManager;
        _fileUpload = fileUpload;
    }

    [Route("/profile")]
    public async Task<IActionResult> Profile() {
        var user = await _userManager.GetUserAsync(User);
        var model = new EditUserModel{
            FullName = user.Name,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Image = user.Image
        };
        return View(model);
    }
    [HttpPost("/edit-profile")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProFile(EditUserModel model, IFormFile? fileImage) {
        if(!ModelState.IsValid) {
            return View("Profile", model);
        }
        var user = await _userManager.GetUserAsync(User);
        // Kiểm tra mật khẩu
        var passwordCheck = await _userManager.CheckPasswordAsync(user, model.PasswordConfirm);
        if(!passwordCheck) {
            TempData["ErrorMessage"] = "Mật khẩu không chính xác";
            return RedirectToAction("Profile", model);
        }
        var fileName = await _fileUpload.UploadFileAsync(fileImage, "UserImages");
        user.Image = fileName;
        user.Name = model.FullName;
        user.Address = model.Address;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        await _userManager.UpdateAsync(user);
        TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công";
        return RedirectToAction("Index", "Home");
    }
}