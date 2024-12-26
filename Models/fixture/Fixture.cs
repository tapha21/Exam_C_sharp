using ges_commande.Models.entity;
using ges_commande.Models.Enum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GesCommande.core;

namespace ges_commande.Models.Fixture
{
    public class Fixtures
    {
        public void Initialize(IServiceProvider serviceProvider, Context context)
        {
            if (!context.Users.Any())
            {
                // Création des utilisateurs
                var user = new User
                {
                    Nom = "Tall",
                    Prenom = "Tapha",
                    Telephone = "123456789",
                    login = "tapha",
                    password = "password",
                    role = Role.Client
                };

                var user2 = new User
                {
                    Nom = "Tall",
                    Prenom = "Talla",
                    Telephone = "123456789",
                    login = "talla",
                    password = "password",
                    role = Role.RS
                };
                
                var user3 = new User
                {
                    Nom = "Tall",
                    Prenom = "taphisco",
                    Telephone = "123456789",
                    login = "taphis",
                    password = "password",
                    role = Role.Comptable
                };
                
                // Ajout des utilisateurs dans la base de données
                context.Users.Add(user);
                context.Users.Add(user2);
                context.Users.Add(user3);


                // Sauvegarde des utilisateurs pour générer les IDs
                context.SaveChanges();

                // Création des clients en associant un UserId existant
                var client = new Client
                {
                    Nom = "Tall",
                    Prenom = "Talla",
                    Telephone = "123456789",
                    Solde = 100.0,
                    Adresse = "123 Main St",
                    UserId = user2.Id  // Utilisation de l'ID de l'utilisateur créé
                };

                // Ajout du client dans la base de données
                context.Clients.Add(client);

                // Création et ajout d'un produit
                var produit = new Produit
                {
                    Libelle = "Product 1",
                    Prix = 10.0,
                    Quantite = 100
                };
                context.Produits.Add(produit);

                // Ajout de livreurs
                var livreur1 = new Livreur
                {
                    Nom = "balde",
                    Prenom = "aliou",
                    Telephone = "0023133",
                    Disponible = true
                };

                var livreur2 = new Livreur
                {
                    Nom = "siby",
                    Prenom = "yaya",
                    Telephone = "0123133",
                    Disponible = false
                };
                
                context.Livreurs.Add(livreur1);
                context.Livreurs.Add(livreur2);

                // Création des commandes
                var commande1 = new Commande
                {
                    montant = 100,
                    dateTime = DateTime.Now,
                    ClientId = user2.Id,
                    CreateAt = DateTime.Now,
                    Etat=Etat.Encours
                };

                var commande2 = new Commande
                {
                    montant = 100,
                    dateTime = DateTime.Now,
                    ClientId = user2.Id,
                    livreurID = livreur2.Id,
                    CreateAt = DateTime.Now,
                    Etat=Etat.Pret_A_LIVRER,
                    PaiementId = 1
                };

                context.Commandes.Add(commande1);
                context.Commandes.Add(commande2);

                // Création des détails de commande
                var detail1 = new DetailCommande
                {
                    CommandeId = commande2.Id,
                    ProduitId = produit.Id,
                    QuantiteComamnde = 10,
                    prixTotal = 100,
                    CreateAt = DateTime.Now
                };

                var detail2 = new DetailCommande
                {
                    CommandeId = commande1.Id,
                    ProduitId = produit.Id,
                    QuantiteComamnde = 10,
                    prixTotal = 100,
                    CreateAt = DateTime.Now
                };

                context.DetailCommandes.Add(detail1);
                context.DetailCommandes.Add(detail2);

                // Création d'un paiement
                var paiement = new Paiement
                {
                    CommandeId = commande2.Id,
                    typePaiement = TypePaiement.Wave,
                    CreateAt = DateTime.Now
                };
                context.Paiements.Add(paiement);

                // Sauvegarde des modifications
                context.SaveChanges();
            }
        }
    }
}
