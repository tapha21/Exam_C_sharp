namespace ges_commande.Models.entity
{
   public class DetailCommande : AbstractEntity
   {
      public int CommandeId { get; set; }
      public Commande Commande { get; set; }
      public int ProduitId { get; set; }
      public Produit Produit { get; set; }
      public int QuantiteComamnde { get; set; }

      public double prixTotal { get; set; }
   }
}