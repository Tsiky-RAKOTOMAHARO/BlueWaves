using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services{
    public class ApprovisionnementServices : IApprovisionnementRepository{
        private readonly IApprovisionnementRepository _approvisionnementRepository;

        public ApprovisionnementServices(IApprovisionnementRepository approvisionnementRepository){
            _approvisionnementRepository = approvisionnementRepository;
        }

        public async Task<Approvisionnement?> GetApprovisionnementById(int idApp){
            return await _approvisionnementRepository.GetApprovisionnementById(idApp);
        }

        public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement(){
            return await _approvisionnementRepository.GetAllApprovisionnement();
        }

        public async Task<IEnumerable<Approvisionnement>> GetApprovisionnementByRefFournisseur(int RefFournisseur){
            return await _approvisionnementRepository.GetApprovisionnementByRefFournisseur(RefFournisseur);
        }

        public async Task AddApprovisionnement(Approvisionnement approvisionnement){
            await _approvisionnementRepository.AddApprovisionnement(approvisionnement);
        }

        public async Task UpdateApprovisionnement(Approvisionnement approvisionnement){
            await _approvisionnementRepository.UpdateApprovisionnement(approvisionnement);
        }

        public async Task DeleteApprovisionnement(Approvisionnement approvisionnement){
            await _approvisionnementRepository.DeleteApprovisionnement(approvisionnement);
        }
    }
}