using System.ComponentModel.DataAnnotations;

namespace ges_commande.Models.Securite
{
    public class LoginModel
    {
         [Required]
        public string Username { get; set; }
         [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}