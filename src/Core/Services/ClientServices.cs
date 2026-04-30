using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services
{
    public class ClientServices{
        private readonly IClientRepository _clientRepository;

        public ClientServices(IClientRepository clientRepository){
            _clientRepository = clientRepository;
        }

        public async Task<Client> GetClientByRef(int Ref){
            if (Ref <= 0)
                throw new ArgumentException("La référence est invalide.");
            return await _clientRepository.GetClientByRef(Ref);
        }

        public async Task<IEnumerable<Client>> GetAllClient()
        {
            return await _clientRepository.GetAllClient();
        }

        public async Task AddClient(string nom, string prenom, string telephone)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom est obligatoire.");
            if (string.IsNullOrWhiteSpace(telephone))
                throw new ArgumentException("Le téléphone est obligatoire.");

            await _clientRepository.AddClient(new Client
            {
                NomClient    = nom.Trim(),
                PrenomClient = prenom?.Trim() ?? string.Empty,
                Telephone    = telephone.Trim()
            });
        }

        public async Task UpdateClient(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.NomClient))
                throw new ArgumentException("Le nom est obligatoire.");
            if (string.IsNullOrWhiteSpace(client.Telephone))
                throw new ArgumentException("Le téléphone est obligatoire.");

            await _clientRepository.UpdateClient(client);
        }

        public async Task DeleteClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException("Le client est introuvable.");
            await _clientRepository.DeleteClient(client);
        }
    }
}