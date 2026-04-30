using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services
{
    public class ProduitServices
    {
        private readonly IProduitRepository _produitRepository;

        public ProduitServices(IProduitRepository produitRepository)
        {
            _produitRepository = produitRepository;
        }

        
         public async Task<Produit> AddProduit(string nom, int prix)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom est obligatoire");

            if (prix < 0)
                throw new ArgumentException("Le prix doit être positif");

            var produit = new Produit
            {
                NomProduit = nom.Trim(),
                Prix = prix,
                Statut = true
            };

            return await _produitRepository.AddProduit(produit);
        }

        public async Task<Produit?> GetProduitByCode(int codeProduit)
        {
            if (codeProduit <= 0)
                throw new ArgumentException("Code invalide");

            return await _produitRepository.GetProduitByCode(codeProduit);
        }

        public async Task<IEnumerable<Produit>> GetAllProduit()
        {
            return await _produitRepository.GetAllProduit();
        }

        public async Task DeleteProduit(int codeProduit)
        {
            if (codeProduit <= 0)
                throw new ArgumentException("Code invalide");

            var produit = await _produitRepository.GetProduitByCode(codeProduit);

            if (produit == null)
                throw new Exception("Produit introuvable");

            await _produitRepository.DeleteProduit(produit);
        }
    }
}