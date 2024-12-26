namespace ges_commande.Models.entity
{
    public class Personne : AbstractEntity
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
    }
}