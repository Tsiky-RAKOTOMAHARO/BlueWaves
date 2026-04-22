using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces{
    public interface IStockRepository{
        Task<Stock> GetStockByNum (int NumStock);

        Task<IEnumrable> GetAllStock();

        Task AddStock(Stock stock);

        void UpdateStock(Stock stock);

        void DeleteStock(Stock stock);
    }
}