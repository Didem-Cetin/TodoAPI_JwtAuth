using System.ComponentModel.DataAnnotations;

namespace TodoAPI_JwtAuth.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Username { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(50)]

        public string Role { get; set; }
    }
}
