using System.ComponentModel.DataAnnotations;

namespace FND.API.Entities
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password_hash { get; set; }
        public DateTimeOffset Created_at { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public int Status { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
