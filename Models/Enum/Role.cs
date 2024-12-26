namespace ges_commande.Models.Enum
{
    public enum Role 
    {
        Client,
        RS,
        Secretaire,
        Comptable
    }

      public static class RoleHelper
    {
        // Méthode pour obtenir une valeur de l'enum à partir d'une chaîne
        public static Role? GetValue(string value)
        {
            foreach (Role role in System.Enum.GetValues(typeof(Role)))  {               // Comparaison insensible à la casse
                if (string.Equals(role.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    return role; // Retourne le rôle correspondant si trouvé
                }
            }
            return null;
        }
    }
}