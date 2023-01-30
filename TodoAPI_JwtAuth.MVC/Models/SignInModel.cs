using System.ComponentModel.DataAnnotations;

namespace TodoAPI_JwtAuth.MVC.Models
{
    public class SignInModel
    {
        [StringLength(50)]
        public string Username { get; set; }
        [MaxLength(16),MinLength(6)]
        public string Password { get; set; }
    }
}
