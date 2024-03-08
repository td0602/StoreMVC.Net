
namespace Store_App.Models.DTO;

public class Status
{
    public int StatusCode {set; get;}

    public string SuccessMessage {set; get;}
    public string ErrorMessage {set; get;}
    public bool isAdmin {set; get;}
}