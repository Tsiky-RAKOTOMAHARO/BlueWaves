using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces{
    public interface ICommandeRepository{
        Task<Commande> GetCommandeByNum(int NumCommande);

        Task<IEnumrable> GetAllCommande();

        Task<IEnumrable> GetCommandeByRefClient();

        Task<IEnumrable> GetCommandeByCodeExport();

        Task AddCommande(Commande commande);

        void UpdateCommande(Commande commande);

        void DeleteCommande(Commande commande);
    }
}