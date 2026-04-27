using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services{
    public class CommandeServices : ICommandeRepository{
        private readonly ICommandeRepository _commandeRepository;

        public CommandeServices(ICommandeRepository commandeRepository){
            _commandeRepository = commandeRepository;
        }

        public async Task<Commande> GetCommandeByNum(int num){
            return await _commandeRepository.GetCommandeByNum(num);
        }

        public async Task<IEnumerable<Commande>> GetAllCommande(){
            return await _commandeRepository.GetAllCommande();
        }

        public async Task<IEnumerable<Commande>> GetCommandeByRefClient(int refClient){
            return await _commandeRepository.GetCommandeByRefClient(refClient);
        }

        public async Task<IEnumerable<Commande>> GetCommandeByCodeExport(int codeExport){
            return await _commandeRepository.GetCommandeByCodeExport(codeExport);
        }

        public async Task AddCommande(Commande commande){
            await _commandeRepository.AddCommande(commande);
        }

        public async Task UpdateCommande(Commande commande){
            await _commandeRepository.UpdateCommande(commande);
        }

        public async Task DeleteCommande(Commande commande){
            await _commandeRepository.DeleteCommande(commande);
        }
    }
}