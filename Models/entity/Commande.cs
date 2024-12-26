using ges_commande.Models.Enum;

namespace ges_commande.Models.entity
{
   public class Commande: AbstractEntity
    {
        public int Id { get; set; }
        public DateTime dateTime{ get; set; }

        public double montant { get; set; }

        public Client Client{ get; set; }
        public int ClientId{ get; set; }
        public Livreur Livreur{ get; set; }
        public int? livreurID{ get; set; }

        public List<DetailCommande> detailCommandes{ get; set; }

        public Paiement? Paiement{ get; set; }   
        public int PaiementId{ get; set; }
        public Etat Etat { get; set; }= Etat.Encours;

       
    }
}