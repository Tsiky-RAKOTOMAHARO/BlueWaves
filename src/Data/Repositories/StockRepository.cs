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
    }
}