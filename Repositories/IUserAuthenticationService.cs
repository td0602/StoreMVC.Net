using Store_App.Models;
using Store_App.Models.DTO;

namespace Store_App.Repositories;

public interface IUserAuthenticationService
{
    Task<Status> LoginAsync(LoginModel model);
    Task<Status> RegisterAsync(RegistrationModel model);
    Task LogoutAsync(); 
}