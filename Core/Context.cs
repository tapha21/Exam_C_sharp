using ges_commande.Models.entity;
using Microsoft.EntityFrameworkCore;

namespace GesCommande.core
{
    public class Context : DbContext
    {
        public required DbSet<User> Users { get; set; }
        public required DbSet<Client> Clients { get; set; }
        public required DbSet<Produit> Produits { get; set; }
        public required DbSet<Commande> Commandes { get; set; }
        public required DbSet<DetailCommande> DetailCommandes { get; set; }
        public required DbSet<Livreur> Livreurs { get; set; }
        public required DbSet<Paiement> Paiements { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=localhost;user=root;database=gesdette;port=3306;password=";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relation entre Client et User (un client peut avoir un utilisateur, mais un utilisateur peut avoir plusieurs clients)
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithMany() // Un utilisateur peut avoir plusieurs clients
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation entre Commande et Client (une commande appartient à un client, mais un client peut avoir plusieurs commandes)
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Client)
                .WithMany(c => c.Commandes)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation entre Commande et Livreur (une commande peut avoir un livreur, mais un livreur peut avoir plusieurs commandes)
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Livreur)
                .WithMany(l => l.Commandes)  // Un livreur peut avoir plusieurs commandes
                .HasForeignKey(c => c.livreurID)
                .OnDelete(DeleteBehavior.SetNull); // On ne supprime pas la commande si le livreur est supprimé, on set à null

            // Relation entre Commande et Paiement (une commande a un paiement, et un paiement est lié à une seule commande)
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Paiement)
                .WithOne(p => p.Commande)
                .HasForeignKey<Paiement>(p => p.CommandeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation entre DetailCommande et Commande (une commande peut avoir plusieurs détails)
            modelBuilder.Entity<DetailCommande>()
                .HasOne(dc => dc.Commande)
                .WithMany(c => c.detailCommandes)
                .HasForeignKey(dc => dc.CommandeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation entre DetailCommande et Produit (un détail de commande est lié à un produit, un produit peut avoir plusieurs détails de commande)
            modelBuilder.Entity<DetailCommande>()
                .HasOne(dc => dc.Produit)
                .WithMany(p => p.DetailCommandes)
                .HasForeignKey(dc => dc.ProduitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
