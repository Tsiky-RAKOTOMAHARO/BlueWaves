using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces{
    public interface IApprovisionnementRepository{
        Task<Approvisionnement> GetApprovisionnementById(int IdApp);

        Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement();

        Task<IEnumerable<Approvisionnement>> GetApprovisionnementRefFournisseur();

        Task AddApprosionnement(Approvisionnement approvisionnement);

        Task UpdateApprovisionnement(Approvisionnement approvisionnement);

        Task DeleteApprovisionnement(Approvisionnement approvisionnement);
    }
}