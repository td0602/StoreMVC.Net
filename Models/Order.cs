using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Store_App.Models;

namespace Store_App.Models;

[Table("Order")]
public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId {set; get;}
    [Display(Name ="Mã đơn hàng")]
    public string OrderCode {set; get;}
    [Display(Name ="Khách hàng")]
    public string UserName {set; get;}
    [Display(Name ="Địa chỉ")]
    public string Address {set; get;}
    [Display(Name ="Số điện thoại")]
    public string PhoneNumber {set; get;}
    [Display(Name ="Email")]
    public string Email {set; get;}
    [Display(Name ="Tổng đơn hàng")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderTotal {set; get;}
    [Display(Name ="Phương thức thanh toán")]
    public string Payment  {set; get;}
    [Display(Name ="Ghi chú")]
    public string Note {set; get;}
    [Display(Name ="Ngày tạo")]
    public DateTime DateCreated {set; get;}

    public List<Cart> carts {get; set;}
}