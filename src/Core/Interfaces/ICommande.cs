using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces{
    public interface ICommandeRepository{
        Task<Commande> GetCommandeByNum(int NumCommande);

        Task<IEnumerable<Commande> > GetAllCommande();

        Task<IEnumerabl<Commande> > GetCommandeByRefClient();

        Task<IEnumerable<Commande> > GetCommandeByCodeExport();

        Task AddCommande(Commande commande);

        Task UpdateCommande(Commande commande);

        Task DeleteCommande(Commande commande);
    }
}