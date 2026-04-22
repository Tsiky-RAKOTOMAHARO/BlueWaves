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
    }
}