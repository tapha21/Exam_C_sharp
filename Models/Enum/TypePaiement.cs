namespace ges_commande.Models.Enum {
    

public enum TypePaiement
{
    Cheque,
    Wave,
    OrangeMoney
    
}
  public static class TypePaiementHelper
    {
        // Méthode pour obtenir une valeur de l'enum à partir d'une chaîne
        public static TypePaiement? GetValue(string value)
        {
            foreach (TypePaiement type in System.Enum.GetValues(typeof(TypePaiement)))
            {
                if (string.Equals(type.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    return type; // Retourne le type de paiement correspondant si trouvé
                }
            }
            return null; // Retourne null si aucun type de paiement n'est trouvé
        }
    }
}