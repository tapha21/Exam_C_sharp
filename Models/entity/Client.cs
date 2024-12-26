using System.ComponentModel.DataAnnotations.Schema;

namespace ges_commande.Models.entity
{
    public class Client : Personne
    {
        public int Id { get; set; }
        public double Solde { get; set; }
        public string Adresse { get; set; }
       
        public List<Commande> Commandes { get; set; }

        public User User { get; set; }
        
        public int UserId { get; set; }

    }
}