using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories{
    public class ClientRepository: IClientRepository{
        
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context){
            _context = context;
        }
    }
}