using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories{
    public class ApprovisionnementRepository: IApprovisionnementRepository{
        
        private readonly AppDbContext _context;

        public ApprovisionnementRepository(AppDbContext context){
            _context = context;
        }

        public async Task<Approvisionnement> GetApprovisionnementById(int IdApp){
            return await _context.Approvisionnement.FindAsync(IdApp);
        }

        public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement(){
            return await _context.Approvisionnement.ToListAsync();
        }

        public async Task<IEnumerable<Approvisionnement>> GetApprovisionnementRefFournisseur(){
            
        }

        public async Task AddApprovisionnement(Approvisionnement approvisionnement){

            await _context.Approvisionnement.AddAsync(approvisionnement);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateApprovisionnement(Approvisionnement approvisionnement){

            _context.Approvisionnement.Update(approvisionnement);
            await _context.SaveChangesAsync();
            
        }

        public async Task DeleteApprovisionnement(Approvisionnement approvisionnement){

            _context.Approvisionnement.Remove(approvisionnement);
            await _context.SaveChangesAsync();

        }
    }
}