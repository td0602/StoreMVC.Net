using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_App.Models;

[Table("UserRole")]
public class UserRole{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserRoleId {set; get;}

    public int UserId {set; get;}
    [ForeignKey("UserId")]
    public User user {set; get;}

    public int RoleId {set; get;}
    [ForeignKey("RoleId")]
    public Role role {set; get;}
}