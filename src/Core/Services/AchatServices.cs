using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;
using System;

namespace Core.Services
{
    public class AchatServices
    {
        private readonly IAchatRepository _achatRepo;
        private readonly IStockProduitRepository _stockProduitRepo;

        public AchatServices(IAchatRepository achatRepo,
            IStockProduitRepository stockProduitRepo)
        {
            _achatRepo = achatRepo;
            _stockProduitRepo = stockProduitRepo;
        }

        public async Task<IEnumerable<Achat>> GetAllAchat()
            => await _achatRepo.GetAllAchat();

        public async Task<Achat?> GetAchatById(int idAchat)
            => await _achatRepo.GetAchatById(idAchat);

        public async Task<IEnumerable<Achat>> GetByProduit(int codeProduit)
            => await _achatRepo.GetAchatByCodeProduit(codeProduit);

        public async Task<IEnumerable<Achat>> GetByStock(int numeroStock)
            => await _achatRepo.GetAchatByStock(numeroStock);

        public async Task<IEnumerable<Achat>> GetByCommande(int numeroCommande)
            => await _achatRepo.GetAchatByNumeroCommande(numeroCommande);

        public async Task AddAchat(Achat achat)
        {
            var ligne = await _stockProduitRepo.GetByLocationAndProduct(
                achat.NumeroStock, achat.CodeProduit);

            if (ligne == null)
                throw new InvalidOperationException("Produit inexistant dans ce stock.");

            if (ligne.Quantite < achat.Quantite)
                throw new InvalidOperationException("Stock insuffisant pour cet achat.");

            ligne.Quantite -= achat.Quantite;
            await _stockProduitRepo.Update(ligne);

            await _achatRepo.AddAchat(achat);
        }

        public async Task UpdateAchat(Achat achat)
        {
            var ancienAchat = await _achatRepo.GetAchatById(achat.IdAchat);
            if (ancienAchat == null)
                throw new InvalidOperationException("Achat introuvable.");

            var delta = achat.Quantite - ancienAchat.Quantite;

            if (delta != 0)
            {
                var ligne = await _stockProduitRepo.GetByLocationAndProduct(
                    achat.NumeroStock, achat.CodeProduit);

                if (ligne == null)
                    throw new InvalidOperationException("Ligne stock introuvable.");

                if (delta > 0 && ligne.Quantite < delta)
                    throw new InvalidOperationException("Stock insuffisant pour augmenter l'achat.");

                ligne.Quantite -= delta;
                await _stockProduitRepo.Update(ligne);
            }

            await _achatRepo.UpdateAchat(achat);
        }

        public async Task DeleteAchat(Achat achat)
        {
            var ligne = await _stockProduitRepo.GetByLocationAndProduct(
                achat.NumeroStock, achat.CodeProduit);

            if (ligne != null)
            {
                ligne.Quantite += achat.Quantite;
                await _stockProduitRepo.Update(ligne);
            }

            await _achatRepo.DeleteAchat(achat);
        }
    }
}