using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace Data.Context{
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Achat> Achat {get; set;}
        public DbSet<Approvisionnement> Approvisionnement {get; set;}
        public DbSet<Client> Client {get; set;}
        public DbSet<Commande> Commande {get; set;}
        public DbSet<Export> Export {get; set;}
        public DbSet<Fournisseur> Fournisseur {get; set;}
        public DbSet<Produit> Produit {get; set;}
        public DbSet<Stock> Stock {get; set;}
    }
}