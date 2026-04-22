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
    }
}