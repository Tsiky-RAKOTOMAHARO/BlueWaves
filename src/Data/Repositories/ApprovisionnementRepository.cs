using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ApprovisionnementRepository : IApprovisionnementRepository
    {
        private readonly AppDbContext _context;

        public ApprovisionnementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Approvisionnement?> GetApprovisionnementById(int idApp)
            => await _context.Approvisionnement
                .Include(a => a.Fournisseur)
                .Include(a => a.Produit)
                .Include(a => a.Stock)
                .FirstOrDefaultAsync(a => a.IdApp == idApp);

        public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement()
            => await _context.Approvisionnement
                .Include(a => a.Fournisseur)
                .Include(a => a.Produit)
                .Include(a => a.Stock)
                .ToListAsync();

        public async Task<IEnumerable<Approvisionnement>> GetApprovisionnementByRefFournisseur(int refFournisseur)
            => await _context.Approvisionnement
                .Where(a => a.RefFournisseur == refFournisseur)
                .Include(a => a.Produit)
                .Include(a => a.Stock)
                .ToListAsync();

        public async Task<IEnumerable<Approvisionnement>> GetApprovisionnementByProduit(int codeProduit)
            => await _context.Approvisionnement
                .Where(a => a.CodeProduit == codeProduit)
                .Include(a => a.Stock)
                .Include(a => a.Fournisseur)
                .ToListAsync();

        public async Task<IEnumerable<Approvisionnement>> GetApprovisionnementByStock(int numeroStock)
            => await _context.Approvisionnement
                .Where(a => a.NumeroStock == numeroStock)
                .Include(a => a.Produit)
                .Include(a => a.Fournisseur)
                .ToListAsync();

        public async Task<int> GetStockTotalByProduit(int codeProduit)
            => await _context.Approvisionnement
                .Where(a => a.CodeProduit == codeProduit)
                .SumAsync(a => a.Quantite);

        public async Task AddApprovisionnement(Approvisionnement approvisionnement)
        {
            await _context.Approvisionnement.AddAsync(approvisionnement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApprovisionnement(Approvisionnement approvisionnement)
        {
            _context.Approvisionnement.Update(approvisionnement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteApprovisionnement(Approvisionnement approvisionnement)
        {
            _context.Approvisionnement.Remove(approvisionnement);
            await _context.SaveChangesAsync();
        }
    }
}