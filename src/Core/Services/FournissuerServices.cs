using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;  

namespace Core.Services{
    public class FournisseurServices : IFournisseurRepository{
        private readonly IFournisseurRepository _fournisseurRepository;

        public FournisseurServices(IFournisseurRepository fournisseurRepository){
            _fournisseurRepository = fournisseurRepository;
        }

        public async Task<Fournisseur> GetFournisseurByref(int refFournisseur){
            return await _fournisseurRepository.GetFournisseurByref(refFournisseur);
        }

        public async Task<IEnumerable<Fournisseur>> GetAllFournisseur(){
            return await _fournisseurRepository.GetAllFournisseur();
        }

        public async Task AddFournisseur(Fournisseur fournisseur){
            await _fournisseurRepository.AddFournisseur(fournisseur);
        }

        public async Task UpdateFournisseur(Fournisseur fournisseur){
            await _fournisseurRepository.UpdateFournisseur(fournisseur);
        }

        public async Task DeleteFournisseur(Fournisseur fournisseur){
            await _fournisseurRepository.DeleteFournisseur(fournisseur);
        }
    }
}