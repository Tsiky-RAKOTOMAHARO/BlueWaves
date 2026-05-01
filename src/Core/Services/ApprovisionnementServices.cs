using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;
using System;

namespace Core.Services{
    
    public class ApprovisionnementServices{
    private readonly IApprovisionnementRepository _approRepo;
    private readonly IStockProduitRepository _stockProduitRepo;

   

    public ApprovisionnementServices(IApprovisionnementRepository approRepo, 
        IStockProduitRepository stockProduitRepo)
    {
        _approRepo = approRepo;
        _stockProduitRepo = stockProduitRepo;
    }


    public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement()
    => await _approRepo.GetAllApprovisionnement();

    public async Task<Approvisionnement?> GetApprovisionnementById(int idApp)
    => await _approRepo.GetApprovisionnementById(idApp);

    public async Task<IEnumerable<Approvisionnement>> GetByProduit(int codeProduit)
    => await _approRepo.GetApprovisionnementByProduit(codeProduit);

    public async Task<IEnumerable<Approvisionnement>> GetByStock(int numeroStock)
    => await _approRepo.GetApprovisionnementByStock(numeroStock);
    public async Task AddApprovisionnement(Approvisionnement appro)
{
    
    await _approRepo.AddApprovisionnement(appro);

   
    var ligne = await _stockProduitRepo.GetByLocationAndProduct(
        appro.NumeroStock, appro.CodeProduit);

    if (ligne == null)
    {
        await _stockProduitRepo.Add(new StockProduit
        {
            NumeroStock = appro.NumeroStock,
            CodeProduit = appro.CodeProduit,
            Quantite = appro.Quantite
        });
    }
    else
    {
        ligne.Quantite += appro.Quantite;
        await _stockProduitRepo.Update(ligne);
    }
}
    public async Task UpdateApprovisionnement(Approvisionnement appro)
        {
            var ancienAppro = await _approRepo.GetApprovisionnementById(appro.IdApp);
            if (ancienAppro == null)
                throw new InvalidOperationException("Approvisionnement introuvable.");

            var delta = appro.Quantite - ancienAppro.Quantite;

            if (delta != 0)
            {
                var ligne = await _stockProduitRepo.GetByLocationAndProduct(
                    appro.NumeroStock, appro.CodeProduit);

                if (ligne == null)
                    throw new InvalidOperationException("Ligne stock introuvable.");

                ligne.Quantite += delta; 
                await _stockProduitRepo.Update(ligne);
            }

            await _approRepo.UpdateApprovisionnement(appro);
        }

    public async Task DeleteApprovisionnement(Approvisionnement appro)
{
    
    var ligne = await _stockProduitRepo.GetByLocationAndProduct(
        appro.NumeroStock, appro.CodeProduit);

    if (ligne != null)
    {
        ligne.Quantite -= appro.Quantite;
        await _stockProduitRepo.Update(ligne);
    }

   
    await _approRepo.DeleteApprovisionnement(appro);
}
}      
}