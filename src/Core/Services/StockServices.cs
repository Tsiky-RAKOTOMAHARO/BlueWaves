using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services{
    public class StockServices : IStockRepository{
        private readonly IStockRepository _stockRepository;

        public StockServices(IStockRepository stockRepository){
            _stockRepository = stockRepository;
        }

        public async Task<Stock?> GetStockByNum(int numStock){
            return await _stockRepository.GetStockByNum(numStock);
        }

        public async Task<IEnumerable<Stock>> GetAllStock(){
            return await _stockRepository.GetAllStock();
        }

        public async Task AddStock(Stock stock){
            await _stockRepository.AddStock(stock);
        }

        public async Task UpdateStock(Stock stock){
            await _stockRepository.UpdateStock(stock);
        }

        public async Task DeleteStock(Stock stock){
            await _stockRepository.DeleteStock(stock);
        }
    }
}