namespace ges_commande.Models.Enum
{
    public enum Etat 
    {
        Payer = 1,
        Encours = 2, 
        Annuler = 3,
        Pret_A_LIVRER = 4,
    }

      public static class EtatHelper
    {
        // Méthode pour obtenir une valeur de l'enum à partir d'une chaîne
        public static Etat? GetValue(string value)
        {
            foreach (Etat etat in System.Enum.GetValues(typeof(Etat)))  {               // Comparaison insensible à la casse
                if (string.Equals(etat.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    return etat; // Retourne le rôle correspondant si trouvé
                }
            }
            return null;
        }
    }
}