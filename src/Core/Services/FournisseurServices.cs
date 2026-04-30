using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services
{
    public class FournisseurServices {
        private readonly IFournisseurRepository _fournisseurRepository;

        public FournisseurServices(IFournisseurRepository fournisseurRepository){
            _fournisseurRepository = fournisseurRepository;
        }

        public async Task<Fournisseur> GetFournisseurByRef(int refFournisseur){
            if (refFournisseur <= 0)
                throw new ArgumentException("La référence est invalide.");
            return await _fournisseurRepository.GetFournisseurByref(refFournisseur);
        }

        public async Task<IEnumerable<Fournisseur>> GetAllFournisseur()
        {
            return await _fournisseurRepository.GetAllFournisseur();
        }

        public async Task AddFournisseur(string nom, string prenom, string telephone){
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom est obligatoire.");
            if (string.IsNullOrWhiteSpace(telephone))
                throw new ArgumentException("Le téléphone est obligatoire.");

            await _fournisseurRepository.AddFournisseur(new Fournisseur
            {
                NomFournisseur       = nom.Trim(),
                PrenomFournisseur    = prenom?.Trim() ?? string.Empty,
                TelephoneFournisseur = telephone.Trim()
            });
        }

        public async Task UpdateFournisseur(Fournisseur fournisseur){
            if (string.IsNullOrWhiteSpace(fournisseur.NomFournisseur))
                throw new ArgumentException("Le nom est obligatoire.");
            if (string.IsNullOrWhiteSpace(fournisseur.TelephoneFournisseur))
                throw new ArgumentException("Le téléphone est obligatoire.");

            await _fournisseurRepository.UpdateFournisseur(fournisseur);
        }

        public async Task DeleteFournisseur(Fournisseur fournisseur){
            if (fournisseur == null)
                throw new ArgumentNullException("Le fournisseur est introuvable.");
            await _fournisseurRepository.DeleteFournisseur(fournisseur);
        }
    }
}