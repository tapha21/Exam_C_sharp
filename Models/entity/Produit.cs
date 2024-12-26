using System.ComponentModel.DataAnnotations.Schema;

namespace ges_commande.Models.entity
{
    public class Produit : AbstractEntity
    {
        public string Libelle { get; set; }
        public double Prix { get; set; }
        public string image { get; set; }
        public int Quantite { get; set; }
        [NotMapped]
        public List<DetailCommande> DetailCommandes { get; set; }
    }
}