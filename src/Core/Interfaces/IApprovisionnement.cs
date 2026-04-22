using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces{
    public interface IApprovisionnementRepository{
        Task<Approvisionnement> GetApprovisionnementById(int IdApp);

        Task<IEnumrable> GetAllApprovisionnement();

        Task<IEnumrable> GetApprovionnementRefFournisseur();

        Task AddApprosionnement(Approvisionnement approvisionnement);

        void UpdateApprovisionnement(Approvisionnement approvisionnement);

        void DeleteApprovisionnement(Approvisionnement approvisionnement);
    }
}