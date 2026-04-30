using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services
{
    public class CommandeServices
    {
        private readonly ICommandeRepository _commandeRepository;

        public CommandeServices(ICommandeRepository commandeRepository)
        {
            _commandeRepository = commandeRepository;
        }

        public async Task<Commande> GetCommandeByNum(int num)
        {
            if (num <= 0)
                throw new ArgumentException("Le numéro de commande est invalide.");
            return await _commandeRepository.GetCommandeByNum(num);
        }

        public async Task<IEnumerable<Commande>> GetAllCommande()
        {
            return await _commandeRepository.GetAllCommande();
        }

        public async Task<IEnumerable<Commande>> GetCommandeByRefClient(int refClient)
        {
            if (refClient <= 0)
                throw new ArgumentException("La référence client est invalide.");
            return await _commandeRepository.GetCommandeByRefClient(refClient);
        }

        public async Task<IEnumerable<Commande>> GetCommandeByCodeExport(int codeExport)
        {
            if (codeExport <= 0)
                throw new ArgumentException("Le code export est invalide.");
            return await _commandeRepository.GetCommandeByCodeExport(codeExport);
        }

        public async Task AddCommande(string destination, DateTime dateCommande, int refClient, int codeExport)
        {
            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("La destination est obligatoire.");
            if (refClient <= 0)
                throw new ArgumentException("Le client est invalide.");
            if (codeExport <= 0)
                throw new ArgumentException("L'export est invalide.");

            await _commandeRepository.AddCommande(new Commande
            {
                Destination    = destination.Trim(),
                DateCommande   = dateCommande,
                RefClient      = refClient,
                CodeExport     = codeExport
            });
        }

        public async Task UpdateCommande(Commande commande)
        {
            if (string.IsNullOrWhiteSpace(commande.Destination))
                throw new ArgumentException("La destination est obligatoire.");
            if (commande.RefClient <= 0)
                throw new ArgumentException("Le client est invalide.");
            if (commande.CodeExport <= 0)
                throw new ArgumentException("L'export est invalide.");

            await _commandeRepository.UpdateCommande(commande);
        }

        public async Task DeleteCommande(Commande commande)
        {
            if (commande == null)
                throw new ArgumentNullException(nameof(commande), "La commande est introuvable.");
            await _commandeRepository.DeleteCommande(commande);
        }
    }
}