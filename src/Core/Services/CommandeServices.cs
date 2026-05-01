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
        {
            return await _commandeRepository.GetAllCommande();
        }

        public async Task<IEnumerable<Commande>> GetCommandeByRefClient(int refClient)
        {
            if (refClient <= 0)
                throw new ArgumentException("Client invalide.");

            return await _commandeRepository.GetCommandeByRefClient(refClient);
        }

        public async Task<IEnumerable<Commande>> GetCommandeByNumeroExport(int numeroExport)
        {
            if (numeroExport <= 0)
                throw new ArgumentException("Export invalide.");

            return await _commandeRepository.GetCommandeByNumeroExport(numeroExport);
        }

        public async Task<Commande> AddCommande(string destination, DateTime dateCommande, int refClient, int numeroExport)
        {
            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("Destination obligatoire.");

            if (refClient <= 0)
                throw new ArgumentException("Client invalide.");

            if (numeroExport <= 0)
                throw new ArgumentException("Export invalide.");

            var commande = new Commande
            {
                Destination = destination.Trim(),
                DateCommande = dateCommande,
                RefClient = refClient,
                NumeroExport = numeroExport
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

            if (commande.NumeroExport <= 0)
                throw new ArgumentException("Export invalide.");

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