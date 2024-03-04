
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Store_App.Models.CustomIdentity;


public class AppUser : IdentityUser<int>
{   
    public string? Image {set; get;}
    public string Name {set; get;}
    public string Address {set; get;}
    public List<Cart>? Carts {set; get;}
    
}