using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_App.Models;

[Table("Role")]
public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId {set; get;}
    public string RoleName {set; get;}

    public List<UserRole> UserRoles {set; get;}
}