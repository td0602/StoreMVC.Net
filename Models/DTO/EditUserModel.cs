using System.ComponentModel.DataAnnotations;

namespace Store_App.Models.DTO;

public class EditUserModel
{
    [Required(ErrorMessage = "Không được để trống!")]
    public string FullName {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    [EmailAddress]
    public string Email {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    public string Address {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    public string PhoneNumber {set; get;}
    [Required(ErrorMessage = "Không được để trống!")]
    public string PasswordConfirm {set; get;}
    public string? Image {get; set;}
    public string? Role {set; get;}
}