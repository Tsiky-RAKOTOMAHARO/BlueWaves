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

        
        public async Task<Client?> GetClientByRefAsync(int clientRef)
        {
            return await _context.Client.FindAsync(clientRef);
        }

        
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.Client.ToListAsync();
        }

        
        public async Task AddClientAsync(Client client)
        {
            await _context.Client.AddAsync(client);
            await _context.SaveChangesAsync(); 
        }


        public async Task UpdateClientAsync(Client client)
        {
            _context.Client.Update(client);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteClientAsync(Client client)
        {
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
        }
    }
}