using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_App.Models;
[Table("Category")]
public class Category
{
    [Key]
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId {set; get;}
    [Display(Name ="Tên thể loại")]
    [Required(ErrorMessage ="Không được để trống!")]
    public string CategoryName {set; get;}

    public List<Book>? books {get; set;}
}