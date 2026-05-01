using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services
{
    public class StockProduitServices
    {
        private readonly IStockProduitRepository _stockProduitRepository;

        public StockProduitServices(IStockProduitRepository stockProduitRepository)
        {
            _stockProduitRepository = stockProduitRepository;
        }

        public async Task<List<StockProduit>> GetAllStockDetails()
        {
            var items = await _stockProduitRepository.GetAll();
            return items.ToList();
        }

        public async Task<int> GetQuantiteDisponible(int codeProduit, int numeroStock)
        {
            var ligne = await _stockProduitRepository.GetByLocationAndProduct(numeroStock, codeProduit);
            return ligne?.Quantite ?? 0;
        }

        public async Task AddOrUpdateStockProduit(int numeroStock, int codeProduit, int quantiteDelta)
        {
            var ligne = await _stockProduitRepository.GetByLocationAndProduct(numeroStock, codeProduit);

            if (ligne == null)
            {
                await _stockProduitRepository.Add(new StockProduit
                {
                    NumeroStock = numeroStock,
                    CodeProduit = codeProduit,
                    Quantite = quantiteDelta
                });
                return;
            }

            ligne.Quantite += quantiteDelta;
            await _stockProduitRepository.Update(ligne);
        }

        public async Task RemoveStockForProduct(int codeProduit, int quantiteDemandee)
        {
            if (quantiteDemandee <= 0)
                throw new ArgumentException("La quantité doit être supérieure à 0.");

            var lignesProduit = (await _stockProduitRepository.GetAll())
                .Where(sp => sp.CodeProduit == codeProduit && sp.Quantite > 0)
                .OrderBy(sp => sp.NumeroStock)
                .ToList();

            var stockTotal = lignesProduit.Sum(sp => sp.Quantite);
            if (stockTotal < quantiteDemandee)
                throw new InvalidOperationException("Stock insuffisant pour ce produit.");

            var restant = quantiteDemandee;
            foreach (var ligne in lignesProduit)
            {
                if (restant == 0)
                    break;

                var retire = Math.Min(ligne.Quantite, restant);
                ligne.Quantite -= retire;
                restant -= retire;
                await _stockProduitRepository.Update(ligne);
            }
        }
    }
}