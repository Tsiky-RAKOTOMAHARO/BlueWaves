using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services{
    public class ClientServices : IClientRepository{
        private readonly IClientRepository _clientRepository;

        public ClientServices(IClientRepository clientRepository){
            _clientRepository = clientRepository;
        }

        public async Task<Client> GetClientByRef(int Ref){
            return await _clientRepository.GetClientByRef(Ref);
        }

        public async Task<IEnumerable<Client>> GetAllClient(){
            return await _clientRepository.GetAllClient();
        }

        public async Task AddClient(Client client){
            await _clientRepository.AddClient(client);
        }

        public async Task UpdateClient(Client client){
            await _clientRepository.UpdateClient(client);
        }

        public async Task DeleteClient(Client client){
            await _clientRepository.DeleteClient(client);
        }
    }
}