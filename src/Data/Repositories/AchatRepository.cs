using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class AchatRepository: IAchatRepository{

    private readonly AppDbContext _context;

        public AchatRepository(AppDbContext context)
        {
            _context = context;
        }
    public async Task<Achat?> GetAchatById(int id)
    {
        return await _context.Achat
            .Include(a => a.Produit)
            .Include(a => a.Commande)
            .Include(a => a.Stock)        
            .FirstOrDefaultAsync(a => a.IdAchat == id);
    }
    
    public async Task<IEnumerable<Achat>> GetAllAchat()
    {
        return await _context.Achat
            .Include(a => a.Produit)
            .Include(a => a.Commande)
            .Include(a => a.Stock)        
            .ToListAsync();
    }

    public async Task<IEnumerable<Achat>> GetAchatByStock(int numeroStock)
    {
        return await _context.Achat
            .Where(a => a.NumeroStock == numeroStock)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Achat>> GetAchatByNumeroCommande(int numCommande)
    {
        return await _context.Achat
            .Include(a => a.Produit)
            .Include(a => a.Commande)
            .Include(a => a.Stock)        
            .Where(a => a.NumeroCommande == numCommande)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Achat>> GetAchatByCodeProduit(int codeProduit)
    {
        return await _context.Achat
            .Include(a => a.Produit)
            .Include(a => a.Commande)
            .Include(a => a.Stock)        
            .Where(a => a.CodeProduit == codeProduit)
            .ToListAsync();
    }
    public async Task AddAchat(Achat achat){
        await _context.Achat.AddAsync(achat);
        await _context.SaveChangesAsync();
    }
    
            
    public async Task UpdateAchat(Achat achat){
        _context.Achat.Update(achat);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAchat(Achat achat)
    {
        _context.Achat.Remove(achat);
        await _context.SaveChangesAsync();
    }
    
    }
}