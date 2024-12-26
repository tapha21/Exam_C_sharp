using ges_commande.Models.Enum;

namespace ges_commande.Models.entity
{
    public class User:Personne
    {
        public string login { get; set; }
        public string password { get; set; }
        public Role role{ get; set; }
    }
}