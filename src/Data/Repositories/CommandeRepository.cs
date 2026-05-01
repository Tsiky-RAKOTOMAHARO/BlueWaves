using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly AppDbContext _context;

        public CommandeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Commande?> GetCommandeByNumero(int numeroCommande)
        {
            return await _context.Commande
                .Include(c => c.Client)
                .Include(c => c.Export)
                .Include(c => c.Achats)
                    .ThenInclude(a => a.Produit)
                .FirstOrDefaultAsync(c => c.NumeroCommande == numeroCommande);
        }

        public async Task<IEnumerable<Commande>> GetAllCommande()
        {
            return await _context.Commande
                .Include(c => c.Client)
                .Include(c => c.Export)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetCommandeByRefClient(int refClient)
        {
            return await _context.Commande
                .Where(c => c.RefClient == refClient)
                .Include(c => c.Export)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetCommandeByNumeroExport(int numeroExport)
        {
            return await _context.Commande
                .Where(c => c.NumeroExport == numeroExport) 
                .Include(c => c.Client)
                .ToListAsync();
        }

        public async Task<Commande> AddCommande(Commande commande)
        {
            await _context.Commande.AddAsync(commande);
            await _context.SaveChangesAsync();
            return commande; 
        }

        public async Task UpdateCommande(Commande commande)
        {
            _context.Commande.Update(commande);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommande(Commande commande)
        {
            _context.Commande.Remove(commande);
            await _context.SaveChangesAsync();
        }
    }
}