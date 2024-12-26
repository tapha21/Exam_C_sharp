
using System.ComponentModel.DataAnnotations.Schema;

namespace ges_commande.Models.entity
{
    public class Livreur : Personne
    {
       public Boolean Disponible { get; set; }
       [NotMapped]
       public List<Commande> Commandes { get; set; }
    }
}