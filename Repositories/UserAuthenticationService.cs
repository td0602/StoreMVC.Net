using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Store_App.Models;
using Store_App.Models.CustomIdentity;
using Store_App.Models.DTO;

namespace Store_App.Repositories;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly SignInManager<AppUser> signInManager;
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<CustomIdentityRole> roleManager;
    public UserAuthenticationService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<CustomIdentityRole> roleManager){
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<Status> LoginAsync(LoginModel model)
    {
        var status = new Status();
        var user = await userManager.FindByNameAsync(model.Username);
        if(user == null) {
            status.StatusCode = 0;
            status.Message = "Tên đăng nhập không đúng";
            return status;
        }
        if(!await userManager.CheckPasswordAsync(user, model.Password)) {
            status.StatusCode = 0;
            status.Message = "Mật khẩu không đúng";
            return status;
        }
        // xác thực người dùng đăng nhập đúng hay không?
        // false: Đây là giá trị cho biết liệu cookie xác thực có được lưu giữ hay không.
        // true: Đây là giá trị cho biết liệu cookie xác thực có được gửi lại với mỗi yêu cầu hay không
        var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
        if(signInResult.Succeeded) {
            var userRoles = await userManager.GetRolesAsync(user); // lấy danh sách vai trò người dùng
            status.isAdmin = userRoles.ToList().Contains("admin");
            // claim là thông tin cụ thể về người dùng, được sử dụng để mô tả các thuộc tính, quyền hạn hoặc các thông tin khác về người dùng
            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName) // thêm claim UserName vào danh sách claim
            };
            foreach (var userRole in userRoles)
            {
                // Với mỗi vai trò của người dùng, một claim mới với loại là ClaimTypes.Role và giá trị là userRole được thêm vào danh sách
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            
            status.StatusCode = 1;
            status.Message = "Đăng nhập thành công";
            return status;
        } else if (signInResult.IsLockedOut) {
            status.StatusCode = 0;
            status.Message = "Tài khoản của bạn đã bị khóa";
            return status;
        } else {
            status.StatusCode = 0;
            status.Message = "Lỗi đăng nhập";
            return status;
        }   
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }

    public async Task<Status> RegisterAsync(RegistrationModel model)
    {
        var status = new Status();
        var userExists = await userManager.FindByNameAsync(model.Username);
        if(userExists != null) {
            status.StatusCode = 0;
            status.Message = "Người dùng đã tồn tại";
            return status;
        }

        AppUser user = new AppUser {
            SecurityStamp = Guid.NewGuid().ToString(),
            Name = model.Name,
            Email=model.Email,
            Address = model.Address,
            UserName=model.Username,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if(!result.Succeeded) {

            foreach(var item in result.Errors) {
                Console.WriteLine("ERROR============ " + item.Description.ToString());
            }

            status.StatusCode = 0;
            status.Message = "Tạo tài khoản thất bại";
            return status;
        }

        // role management
        if(!await roleManager.RoleExistsAsync(model.Role)) {
            await roleManager.CreateAsync(new CustomIdentityRole(model.Role));
        }
        if(await roleManager.RoleExistsAsync(model.Role)) {
            await userManager.AddToRoleAsync(user, model.Role);
        }

        status.StatusCode = 1;
        status.Message = "Tạo tài khoản thành công";
        return status;
    }
}