using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces{
    public interface IProduitRepository{
    Task<Produit> AddProduit(Produit produit);
    Task<Produit?> GetProduitByCode(int codeProduit);
    Task<IEnumerable<Produit>> GetAllProduit();
    Task DeleteProduit(Produit produit);
}
}