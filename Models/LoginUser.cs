using System.ComponentModel.DataAnnotations;

namespace shoolnew.Models
{
    public class LoginUser
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
