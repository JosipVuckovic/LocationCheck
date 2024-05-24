namespace LocationCheck.API.Models;

public class ErrorModel
{
    public int StatusCode { get; set; }
    public string Status { get; set; }
    public string ErrorMessage { get; set; }
}