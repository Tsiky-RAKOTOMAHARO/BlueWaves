using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces; 

namespace Core.Services{
    public class AchatServices : IAchatRepository{
        private readonly IAchatRepository _achatRepository;

        public AchatServices(IAchatRepository achatRepository){
            _achatRepository = achatRepository;
        }

        public async Task<Achat> GetAchatById(int Id){
            return await _achatRepository.GetAchatById(Id);
        }

        public async Task<IEnumerable<Achat>> GetAllAchat(){
            return await _achatRepository.GetAllAchat();
        }

        public async Task<IEnumerable<Achat>> GetAchatByNumeroCommande(int numCommande){
            return await _achatRepository.GetAchatByNumeroCommande(numCommande);
        }

        public async Task UpdateAchat(Achat achat){
            await _achatRepository.UpdateAchat(achat);
        }

        public async Task DeleteAchat(Achat achat){
            await _achatRepository.DeleteAchat(achat);
        }
    }
}