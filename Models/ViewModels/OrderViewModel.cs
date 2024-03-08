using Store_App.Models.CustomIdentity;
using Store_App.Models.DTO;

namespace Store_App.Models.ViewModels;

public class OrderViewModel {
    public OrderModel OrderView {get; set;}
    public List<Cart> Carts {get; set;}
    public AppUser User {get; set;}
}