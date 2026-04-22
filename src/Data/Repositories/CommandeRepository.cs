using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories{
    public class CommandeRepository: ICommandeRepository{
        
        private readonly AppDbContext _context;

        public CommandeRepository(AppDbContext context){
            _context = context;
        }
    }
}