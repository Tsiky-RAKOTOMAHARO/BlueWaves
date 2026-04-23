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


        public async Task<Fournisseur?> GetFournisseurByref(int refFournisseur) =>
            
            await _context.Fournisseur.FindAsync(refFournisseur);


        public async Task<IEnumerable<Fournisseur>> GetAllFournisseur() => await _context.Fournisseur.ToListAsync();


        public async Task AddFournisseur(Fournisseur fournisseur){
            await _context.Fournisseur.AddAsync(fournisseur);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFournisseur(Fournisseur fournisseur){
            _context.Fournisseur.Update(fournisseur);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFournisseur(Fournisseur fournisseur){
            _context.Fournisseur.Remove(fournisseur);
            await _context.SaveChangesAsync();
        }
    }
}