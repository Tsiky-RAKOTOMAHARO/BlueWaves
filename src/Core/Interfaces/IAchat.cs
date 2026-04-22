using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces{
    public interface IAchatRepository{
        Task<Achat> GetAchatById(int Id);

        Task<IEnumerable<Achat>> GetAllAchat();

        Task<IEnumerable<Achat>> GetAchatByNumeroCommande();

        Task UpdateAchat(Achat achat);

        Task DeleteAchat(Achat achat);
        
    }
}