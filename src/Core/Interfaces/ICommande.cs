using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICommandeRepository
    {
        Task<Commande?> GetCommandeByNumero(int numeroCommande);
        Task<IEnumerable<Commande>> GetAllCommande();
        Task<IEnumerable<Commande>> GetCommandeByRefClient(int refClient);
        Task<Commande> AddCommande(Commande commande);
        Task UpdateCommande(Commande commande);
        Task DeleteCommande(Commande commande);
    }
}