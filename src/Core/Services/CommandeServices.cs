using System;
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

        public async Task<Commande?> GetCommandeByNumero(int numeroCommande)
        {
            if (numeroCommande <= 0)
                throw new ArgumentException("Numéro invalide.");

            return await _commandeRepository.GetCommandeByNumero(numeroCommande);
        }

        public async Task<IEnumerable<Commande>> GetAllCommande()
            => await _commandeRepository.GetAllCommande();

        public async Task<IEnumerable<Commande>> GetCommandeByRefClient(int refClient)
        {
            if (refClient <= 0)
                throw new ArgumentException("Client invalide.");

            return await _commandeRepository.GetCommandeByRefClient(refClient);
        }

        public async Task<Commande> AddCommande(string destination, DateTime dateCommande, int delai, int refClient)
        {
            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("Destination obligatoire.");

            if (refClient <= 0)
                throw new ArgumentException("Client invalide.");

            if (delai <= 0)
                throw new ArgumentException("Délai invalide.");

            var commande = new Commande
            {
                Destination  = destination.Trim(),
                DateCommande = dateCommande,
                Delai        = delai,
                RefClient    = refClient
            };

            return await _commandeRepository.AddCommande(commande);
        }

        public async Task UpdateCommande(Commande commande)
        {
            if (commande == null)
                throw new ArgumentNullException(nameof(commande));

            if (string.IsNullOrWhiteSpace(commande.Destination))
                throw new ArgumentException("Destination obligatoire.");

            if (commande.RefClient <= 0)
                throw new ArgumentException("Client invalide.");

            if (commande.Delai <= 0)
                throw new ArgumentException("Délai invalide.");

            await _commandeRepository.UpdateCommande(commande);
        }

        public async Task DeleteCommande(Commande commande)
        {
            if (commande == null)
                throw new ArgumentNullException(nameof(commande));

            await _commandeRepository.DeleteCommande(commande);
        }
    }
}