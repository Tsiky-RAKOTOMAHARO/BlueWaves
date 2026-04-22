using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces{
    public interface IProduitRepository{
        Task<Produit> GetProduitByCode (int CodeProduit);

        Task<IEnumrable> GetAllProduit();

        Task<IEnumrable> GetProduitByNumStock();

        Task AddProduit(Produit produit);

        void UpdateProduit(Produit produit);

        void DeleteProduit(Produit produit);
    }
}