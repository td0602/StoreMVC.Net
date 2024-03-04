using System.ComponentModel.DataAnnotations;

namespace Store_App.Models.DTO;

public class LoginModel
{
    [Required(ErrorMessage ="Không được để trống!")]
    public string Username {set; get;}
    [Required(ErrorMessage ="Không được để trống!")]
    public string Password {set; get;}
}