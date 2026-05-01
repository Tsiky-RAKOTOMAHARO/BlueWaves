using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces{

public interface IStockProduitRepository
{
    Task<IEnumerable<StockProduit>> GetAll();
    Task<StockProduit?> GetByLocationAndProduct(int numeroStock, int codeProduit);
    Task Update(StockProduit stockProduit);
    Task Add(StockProduit stockProduit);
}
}