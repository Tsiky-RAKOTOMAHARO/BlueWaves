using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories{
    public class AchatRepository: IAchatRepository{
        private readonly AppDbContext _context;

        //Constructeur
        public AchatRepository(AppDbContext context){
            _context = context;
        }

        //Les methodes d'achat
        public async Task<Achat> GetAchatById(int Id){
            
            return await _context.Achat.FindAsync(Id);
    
        }

        public async Task<IEnumerable<Achat>> GetAllAchat(){

            return await _context.Achat.ToListAsync();

        }

        public async Task<IEnumerable<Achat>> GetAchatByNumeroCommande(){
            
        }

        public async Task UpdateAchat(Achat achat){
            
            _context.Achat.Update(achat);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAchat(Achat achat){
            
            _context.Achat.Remove(achat);
            await _context.SaveChangesAsync();
        }
    }
}