using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class RegisterModel
{
    [Required(ErrorMessage = "User name is requerid")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Email is requerid")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is requerid")]
    public string? Password { get; set; }
}
