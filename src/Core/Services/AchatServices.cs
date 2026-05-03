using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services
{
    public class AchatServices
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AchatServices(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<IEnumerable<Achat>> GetAllAchat()
        {
            using var scope = _scopeFactory.CreateScope();
            var achatRepo = scope.ServiceProvider.GetRequiredService<IAchatRepository>();
            return await achatRepo.GetAllAchat();
        }

        public async Task<Achat?> GetAchatById(int idAchat)
        {
            using var scope = _scopeFactory.CreateScope();
            var achatRepo = scope.ServiceProvider.GetRequiredService<IAchatRepository>();
            return await achatRepo.GetAchatById(idAchat);
        }

        public async Task<IEnumerable<Achat>> GetByProduit(int codeProduit)
        {
            using var scope = _scopeFactory.CreateScope();
            var achatRepo = scope.ServiceProvider.GetRequiredService<IAchatRepository>();
            return await achatRepo.GetAchatByCodeProduit(codeProduit);
        }

        public async Task<IEnumerable<Achat>> GetByStock(int numeroStock)
        {
            using var scope = _scopeFactory.CreateScope();
            var achatRepo = scope.ServiceProvider.GetRequiredService<IAchatRepository>();
            return await achatRepo.GetAchatByStock(numeroStock);
        }

        public async Task<IEnumerable<Achat>> GetByCommande(int numeroCommande)
        {
            using var scope = _scopeFactory.CreateScope();
            var achatRepo = scope.ServiceProvider.GetRequiredService<IAchatRepository>();
            return await achatRepo.GetAchatByNumeroCommande(numeroCommande);
        }

        public async Task AddAchat(Achat achat)
        {
            using var scope = _scopeFactory.CreateScope();
            var achatRepo = scope.ServiceProvider.GetRequiredService<IAchatRepository>();
            var stockRepo = scope.ServiceProvider.GetRequiredService<IStockProduitRepository>();

            var ligne = await stockRepo.GetByLocationAndProduct(
                achat.NumeroStock, achat.CodeProduit);

            if (ligne == null)
                throw new InvalidOperationException("Produit inexistant dans ce stock.");

            if (ligne.Quantite < achat.Quantite)
                throw new InvalidOperationException("Stock insuffisant pour cet achat.");

            ligne.Quantite -= achat.Quantite;
            await stockRepo.Update(ligne);

            await achatRepo.AddAchat(achat);
        }

        public async Task UpdateAchat(Achat achat)
        {
            using var scope = _scopeFactory.CreateScope();
            var achatRepo = scope.ServiceProvider.GetRequiredService<IAchatRepository>();
            var stockRepo = scope.ServiceProvider.GetRequiredService<IStockProduitRepository>();

            var ancienAchat = await achatRepo.GetAchatById(achat.IdAchat);
            if (ancienAchat == null)
                throw new InvalidOperationException("Achat introuvable.");

            var delta = achat.Quantite - ancienAchat.Quantite;

            if (delta != 0)
            {
                var ligne = await stockRepo.GetByLocationAndProduct(
                    achat.NumeroStock, achat.CodeProduit);

                if (ligne == null)
                    throw new InvalidOperationException("Ligne stock introuvable.");

                if (delta > 0 && ligne.Quantite < delta)
                    throw new InvalidOperationException("Stock insuffisant pour augmenter l'achat.");

                ligne.Quantite -= delta;
                await stockRepo.Update(ligne);
            }

            await achatRepo.UpdateAchat(achat);
        }

        public async Task DeleteAchat(Achat achat)
        {
            using var scope = _scopeFactory.CreateScope();
            var achatRepo = scope.ServiceProvider.GetRequiredService<IAchatRepository>();
            var stockRepo = scope.ServiceProvider.GetRequiredService<IStockProduitRepository>();

            var ligne = await stockRepo.GetByLocationAndProduct(
                achat.NumeroStock, achat.CodeProduit);

            if (ligne != null)
            {
                ligne.Quantite += achat.Quantite;
                await stockRepo.Update(ligne);
            }

            await achatRepo.DeleteAchat(achat);
        }
    }
}