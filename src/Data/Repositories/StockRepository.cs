using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories{
    public class StockRepository: IStockRepository{
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context){
            _context = context;
        }


        public async Task<Stock?> GetStockByNum(int numStock) => await _context.Stock.FindAsync(numStock);

        public async Task<IEnumerable<Stock>> GetAllStock(){
        _context.ChangeTracker.Clear();  
        return await _context.Stock
        .Include(s => s.StockProduits)
            .ThenInclude(sp => sp.Produit)
        .ToListAsync();
        }

        public async Task AddStock(Stock stock){
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStock(Stock stock){
            _context.Stock.Update(stock);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStock(Stock stock){
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
        }
    }
}