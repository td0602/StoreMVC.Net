using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Store_App.Models.CustomIdentity;

namespace Store_App.Models;

[Table("Cart")]
public class Cart 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartId {get; set;}
    [Display(Name ="Tên sách")]
    public string BookName {set; get;}
    [Display(Name ="Số lượng")]
    public int Quantity {set; get;}
    [Display(Name ="Ảnh")]
    public string? Image {set; get;}
    [Display(Name ="Giá")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price {set; get;}
    [Display(Name ="Tổng")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total {set; get;}
    public bool Status {set; get;}

    public int AppUserId {set; get;}
    [ForeignKey("AppUserId")]
    public AppUser appUser {set; get;}

    public int BookId {set; get;}
    [ForeignKey("BookId")]
    public Book book {set; get;}

    public int? OrderId {set; get;}
    [ForeignKey("OrderId")]
    public Order? order {get; set;}

}