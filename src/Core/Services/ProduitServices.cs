using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services{
    public class ProduitServices : IProduitRepository{
        private readonly IProduitRepository _produitRepository;

        public ProduitServices(IProduitRepository produitRepository){
            _produitRepository = produitRepository;
        }

        public async Task<Produit?> GetProduitByCode(int codeProduit){
            return await _produitRepository.GetProduitByCode(codeProduit);
        }

        public async Task<IEnumerable<Produit>> GetAllProduit(){
            return await _produitRepository.GetAllProduit();
        }

        public async Task<IEnumerable<Produit>> GetProduitByNumStock(int numStock){
            return await _produitRepository.GetProduitByNumStock(numStock);
        }

        public async Task AddProduit(Produit produit){
            await _produitRepository.AddProduit(produit);
        }

        public async Task UpdateProduit(Produit produit){
            await _produitRepository.UpdateProduit(produit);
        }

        public async Task DeleteProduit(Produit produit){
            await _produitRepository.DeleteProduit(produit);
        }
    }
}