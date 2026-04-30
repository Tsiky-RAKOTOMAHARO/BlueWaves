using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services{
    public class StockServices{
        private readonly IStockRepository _stockRepository;

        public StockServices(IStockRepository stockRepository){
            _stockRepository = stockRepository;
        }

        public async Task<Stock?> GetStockByNum(int numStock){
            if (numStock <= 0)
                throw new ArgumentException("Le numéro de stock est invalide.");

            return await _stockRepository.GetStockByNum(numStock);
        }

        public async Task<IEnumerable<Stock>> GetAllStock(){
            return await _stockRepository.GetAllStock();
        }

        public async Task AddStock(string nom){
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Nom obligatoire");

            var stock = new Stock{
                NomStock = nom.Trim(), 
            };

            await _stockRepository.AddStock(stock);
        }

        public async Task UpdateStock(Stock stock){
            if (stock == null)
                throw new ArgumentNullException(nameof(stock));

            if (string.IsNullOrWhiteSpace(stock.NomStock))
                throw new ArgumentException("Le nom du stock est obligatoire.");

            await _stockRepository.UpdateStock(stock);
        }

        public async Task DeleteStock(Stock stock){
            if (stock == null)
                throw new ArgumentNullException(nameof(stock), "Le stock est introuvable.");

            await _stockRepository.DeleteStock(stock);
        }
    }
}