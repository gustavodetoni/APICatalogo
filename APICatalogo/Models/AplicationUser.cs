using Microsoft.AspNetCore.Identity;

namespace APICatalogo.Models
{
    public class AplicationUser : IdentityUser
    {
        public string? RefeshToken { get; set; }
        public DateTime RefeshTokenExpiryTime { get; set; }
    }
}
