using System.ComponentModel.DataAnnotations;

namespace Store_App.Models.DTO;

public class OrderModel 
{
    [Required(ErrorMessage ="Không được để trống!")]
    public string FullName {set; get;}
    [Required(ErrorMessage ="Không được để trống!")]
    public string Payment {set; get;}
    [Required(ErrorMessage ="Không được để trống!")]
    [EmailAddress(ErrorMessage ="Định dạng Email không đúng")]
    public string Email {set; get;}
    [Required(ErrorMessage ="Không được để trống!")]
    public string PhoneNumber {set; get;}
    [Required(ErrorMessage ="Không được để trống!")]
    public string Address {set; get;}
    public string? Note {set; get;}
    public List<Cart> Carts {set; get;}
}