using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories{
    public class FournisseurRepository: IFournisseurRepository{
        private readonly AppDbContext _context;

        public FournisseurRepository(AppDbContext context){
            _context = context;
        }

        public async Task<Fournisseur> GetFournisseurByref (int RefFournisseur){
            
        }

        public async Task<IEnumerable<Fournisseur> > GetAllFournisseur(){
            
        }

        public async Task AddFournisseur(Fournisseur fournisseur){
            
        }

        public async Task UpdateFournisseur(Fournisseur fournisseur){
            
        }

        public async Task DeleteFournisseur(Fournisseur fournisseur){
            
        }
    }
}