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

        public async Task<Stock> GetStockByNum (int NumStock){
            
        }

        public async Task<IEnumerable<Stock>> GetAllStock(){
            
        }

        public async Task AddStock(Stock stock){
            
        }

        public async Task UpdateStock(Stock stock){
            
        }

        public async Task DeleteStock(Stock stock){
            
        }
    }
}