using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories;

public class StockProduitRepository : IStockProduitRepository
{
    private readonly AppDbContext _context;

    public StockProduitRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<StockProduit>> GetAll()
    {
        return await _context.StockProduits
            .Include(sp => sp.Stock)
            .Include(sp => sp.Produit)
            .ToListAsync();
    }

    public async Task<StockProduit?> GetByLocationAndProduct(int numeroStock, int codeProduit)
    {
        return await _context.StockProduits
            .FirstOrDefaultAsync(sp => sp.NumeroStock == numeroStock && sp.CodeProduit == codeProduit);
    }

    public async Task Update(StockProduit stockProduit)
    {
        _context.StockProduits.Update(stockProduit);
        await _context.SaveChangesAsync();
    }

    public async Task Add(StockProduit stockProduit)
    {
        await _context.StockProduits.AddAsync(stockProduit);
        await _context.SaveChangesAsync();
    }
}