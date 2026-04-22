using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces{
    public interface IFournisseurRepository{
        Task<Fournisseur> GetFournisseurByref (int RefFournisseur);

        Task<IEnumrable> GetAllFournisseur();

        Task AddFournisseur(Fournisseur fournisseur);

        void UpdateFournisseur(Fournisseur fournisseur);

        void DeleteFournisseur(Fournisseur fournisseur);
    }
}