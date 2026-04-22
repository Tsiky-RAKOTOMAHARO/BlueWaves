using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces{
    public interface IAchatRepository{
        Task<Achat> GetAchatById(int Id);

        Task<IEnumrable> GetAllAchat();

        Task<IEnumrable> GetAchatByNumeroCommande();

        void UpdateAchat(Achat achat);

        void DeleteAchat(Achat achat);
        
    }
}