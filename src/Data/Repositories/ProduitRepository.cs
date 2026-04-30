using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProduitRepository : IProduitRepository
    {
        private readonly AppDbContext _context;

        public ProduitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Produit> AddProduit(Produit produit)
        {
            await _context.Produit.AddAsync(produit);
            await _context.SaveChangesAsync();
            return produit;
        }

        public async Task<Produit?> GetProduitByCode(int codeProduit)
            => await _context.Produit.FindAsync(codeProduit);

        public async Task<IEnumerable<Produit>> GetAllProduit()
            => await _context.Produit.ToListAsync();

        public async Task DeleteProduit(Produit produit)
        {
            _context.Produit.Remove(produit);
            await _context.SaveChangesAsync();
        }
    }
}