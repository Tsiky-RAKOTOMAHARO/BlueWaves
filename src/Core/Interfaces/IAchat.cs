using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces{
    public interface IAchatRepository{
        Task<Achat?> GetAchatById(int Id);

        Task<IEnumerable<Achat>> GetAllAchat();

        Task<IEnumerable<Achat>> GetAchatByNumeroCommande(int numCommande);

        Task<IEnumerable<Achat>> GetAchatByCodeProduit(int codeProduit);

        Task<IEnumerable<Achat>> GetAchatByStock(int numeroStock);

        Task AddAchat(Achat achat);
        Task UpdateAchat(Achat achat);

        Task DeleteAchat(Achat achat);
        
    }
}