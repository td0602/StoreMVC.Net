using Microsoft.AspNetCore.Identity;

namespace Store_App.Models.CustomIdentity;

public class CustomIdentityRole : IdentityRole<int>
{
    public CustomIdentityRole() : base() {
        
    }
    public CustomIdentityRole(string roleName) : base(roleName)
    {
        this.Name = roleName;
    }
}