using System.ComponentModel.DataAnnotations;

namespace Store_App.Models.DTO;

public class RegistrationModel
{
    [Required(ErrorMessage = "Không được để trống!")]
    public string FullName {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    [EmailAddress]
    public string Email {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    public string Username {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    public string Address {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    public string PhoneNumber {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    public string Password {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp!")]
    public string PasswordConfirm {set; get;}
    public string? Role {set; get;}
}