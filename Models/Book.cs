using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace Store_App.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId {set; get;}
        [Required(ErrorMessage ="Không được để trống!")]
        [Display(Name = "Tên sách")]
        public string BookName {set; get;}
        [Required(ErrorMessage ="Không được để trống!")]
        [Display(Name = "Giá")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price {get; set;}
        [Required(ErrorMessage ="Không được để trống!")]
        [Display(Name = "Tác giả")]
        public string Author {get; set;}
        [Required(ErrorMessage ="Không được để trống!")]
        [Display(Name = "Mô tả")]
        public string Description {get; set;}
        public string? Image {get; set;}

        public List<Cart>? Carts {set; get;}

        public int CategoryId {set; get;}
        [ForeignKey("CategoryId")]
        public Category? Category {set; get;}
    }
}