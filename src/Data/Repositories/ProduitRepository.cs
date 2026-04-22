using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories{
    public class ProduitRepository: IProduitRepository{
        private readonly AppDbContext _context;

        public ProduitRepository(AppDbContext context){
            _context = context;
        }

        public async Task<Produit> GetProduitByCode (int CodeProduit){
            
        }

        public async Task<IEnumerable<Produit>> GetAllProduit(){
            
        }

        public async Task<IEnumerable<Produit>> GetProduitByNumStock(){
            
        }

        public async Task AddProduit(Produit produit){
            
        }

        public async Task UpdateProduit(Produit produit){
            
        }

        public async Task DeleteProduit(Produit produit){
            
        }
    }
}