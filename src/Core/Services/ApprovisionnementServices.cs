using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services
{
    public class ApprovisionnementServices
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ApprovisionnementServices(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement()
        {
            using var scope = _scopeFactory.CreateScope();
            var approRepo = scope.ServiceProvider.GetRequiredService<IApprovisionnementRepository>();
            return await approRepo.GetAllApprovisionnement();
        }

        public async Task<Approvisionnement?> GetApprovisionnementById(int idApp)
        {
            using var scope = _scopeFactory.CreateScope();
            var approRepo = scope.ServiceProvider.GetRequiredService<IApprovisionnementRepository>();
            return await approRepo.GetApprovisionnementById(idApp);
        }

        public async Task<IEnumerable<Approvisionnement>> GetByProduit(int codeProduit)
        {
            using var scope = _scopeFactory.CreateScope();
            var approRepo = scope.ServiceProvider.GetRequiredService<IApprovisionnementRepository>();
            return await approRepo.GetApprovisionnementByProduit(codeProduit);
        }

        public async Task<IEnumerable<Approvisionnement>> GetByStock(int numeroStock)
        {
            using var scope = _scopeFactory.CreateScope();
            var approRepo = scope.ServiceProvider.GetRequiredService<IApprovisionnementRepository>();
            return await approRepo.GetApprovisionnementByStock(numeroStock);
        }

        public async Task AddApprovisionnement(Approvisionnement appro)
        {
            using var scope = _scopeFactory.CreateScope();
            var approRepo  = scope.ServiceProvider.GetRequiredService<IApprovisionnementRepository>();
            var stockRepo  = scope.ServiceProvider.GetRequiredService<IStockProduitRepository>();

            await approRepo.AddApprovisionnement(appro);

            var ligne = await stockRepo.GetByLocationAndProduct(
                appro.NumeroStock, appro.CodeProduit);

            if (ligne == null)
            {
                await stockRepo.Add(new StockProduit
                {
                    NumeroStock = appro.NumeroStock,
                    CodeProduit = appro.CodeProduit,
                    Quantite    = appro.Quantite
                });
            }
            else
            {
                ligne.Quantite += appro.Quantite;
                await stockRepo.Update(ligne);
            }
        }

        public async Task UpdateApprovisionnement(Approvisionnement appro)
        {
            using var scope = _scopeFactory.CreateScope();
            var approRepo  = scope.ServiceProvider.GetRequiredService<IApprovisionnementRepository>();
            var stockRepo  = scope.ServiceProvider.GetRequiredService<IStockProduitRepository>();

            var ancienAppro = await approRepo.GetApprovisionnementById(appro.IdApp);
            if (ancienAppro == null)
                throw new InvalidOperationException("Approvisionnement introuvable.");

            var delta = appro.Quantite - ancienAppro.Quantite;

            if (delta != 0)
            {
                var ligne = await stockRepo.GetByLocationAndProduct(
                    appro.NumeroStock, appro.CodeProduit);

                if (ligne == null)
                    throw new InvalidOperationException("Ligne stock introuvable.");

                ligne.Quantite += delta;
                await stockRepo.Update(ligne);
            }

            await approRepo.UpdateApprovisionnement(appro);
        }

        public async Task DeleteApprovisionnement(Approvisionnement appro)
        {
            using var scope = _scopeFactory.CreateScope();
            var approRepo  = scope.ServiceProvider.GetRequiredService<IApprovisionnementRepository>();
            var stockRepo  = scope.ServiceProvider.GetRequiredService<IStockProduitRepository>();

            var ligne = await stockRepo.GetByLocationAndProduct(
                appro.NumeroStock, appro.CodeProduit);

            if (ligne != null)
            {
                ligne.Quantite -= appro.Quantite;
                await stockRepo.Update(ligne);
            }

            await approRepo.DeleteApprovisionnement(appro);
        }
    }
}