using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;
using System;

namespace Core.Services
{
    public class ApprovisionnementServices
    {
        private readonly IApprovisionnementRepository _approvisionnementRepository;
        private readonly ProduitServices _produitService; 
        

        public ApprovisionnementServices(
            IApprovisionnementRepository approvisionnementRepository, 
            ProduitServices produitService)
        {
            _approvisionnementRepository = approvisionnementRepository;
            _produitService = produitService;
        }

        public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement()
        {
            return await _approvisionnementRepository.GetAllApprovisionnement();
        }

        public async Task AddApprovisionnement(Approvisionnement approvisionnement)
        {
            if (approvisionnement.Quantite <= 0)
                throw new ArgumentException("Quantité invalide");

            if (string.IsNullOrWhiteSpace(approvisionnement.Certificat))
                throw new ArgumentException("Certificat obligatoire");

            var produit = await _produitService.GetProduitByCode(approvisionnement.CodeProduit);

            if (produit == null)
                throw new Exception("Produit introuvable");

            await _approvisionnementRepository.AddApprovisionnement(approvisionnement);
        }

        public async Task DeleteApprovisionnement(Approvisionnement approvisionnement)
        {
            await _approvisionnementRepository.DeleteApprovisionnement(approvisionnement);
        }
    }
}