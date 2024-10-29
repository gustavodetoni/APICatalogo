using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage ="User name is requerid")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is requerid")]
        public string? Password { get; set; }
    }
}
