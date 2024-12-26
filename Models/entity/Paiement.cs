using ges_commande.Models.Enum;

namespace ges_commande.Models.entity
{
   public  class Paiement :  AbstractEntity {

  public TypePaiement typePaiement { get; set; }
  public Commande Commande { get; set; }
  public int CommandeId { get; set; }
   }
}