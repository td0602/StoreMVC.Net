using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_App.Models;

[Table("User")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId {set; get;}
    public string UserName {set; get;}
    public string Password {set; get;}
    public string? Image {set; get;}
    public string FullName {set; get;}
    public string? Email {set; get;}
    public string PhoneNumber {set; get;}
    public string Address {set; get;}

    public List<UserRole> UserRoles {set; get;}
    public List<Cart> Carts {set; get;}

}