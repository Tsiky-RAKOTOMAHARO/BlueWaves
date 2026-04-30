using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IApprovisionnementRepository
    {
        Task<Approvisionnement> GetApprovisionnementById(int idApp);

        Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement();

        Task<IEnumerable<Approvisionnement>> GetApprovisionnementByRefFournisseur(int refFournisseur);

        Task<IEnumerable<Approvisionnement>> GetApprovisionnementByProduit(int codeProduit);

        Task<IEnumerable<Approvisionnement>> GetApprovisionnementByStock(int numeroStock); 

        Task<int> GetStockTotalByProduit(int codeProduit); 

        Task AddApprovisionnement(Approvisionnement approvisionnement);

        Task UpdateApprovisionnement(Approvisionnement approvisionnement);

        Task DeleteApprovisionnement(Approvisionnement approvisionnement);
    }
}