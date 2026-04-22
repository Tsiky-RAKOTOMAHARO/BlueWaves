using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces{
    public interface IClientRepository{
        Task<Client> GetClientByRef (int Ref);

        Task<IEnumrable> GetAllClient();

        Task AddClient(Client client);

        void UpdateClient(Client client);

        void DeleteClient(Client client);

        // Autre methode specifique 
    }
}