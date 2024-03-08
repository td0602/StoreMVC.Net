using System.ComponentModel.DataAnnotations;

namespace Store_App.Models.DTO;

public class RegistrationModel
{
    [Required]
    public string Name {set; get;}
    [Required]
    [EmailAddress]
    public string Email {set; get;}
    [Required]
    public string Username {set; get;}
    [Required]
    public string Address {set; get;}
    [Required]
    [Phone]
    public string PhoneNumber {set; get;}
    [Required]
    public string Password {set; get;}
    [Required]
    [Compare("Password")]
    public string PasswordConfirm {set; get;}
    public string? Role {set; get;}
}