using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories{
    public class CommandeRepository: ICommandeRepository{
        
        private readonly AppDbContext _context;

        public CommandeRepository(AppDbContext context){
            _context = context;
        }

        public async Task<Commande> GetCommandeByNum(int NumCommande){
            
        }

        public async Task<IEnumerable<Commande> > GetAllCommande(){
            
        }

        public async Task<IEnumerabl<Commande> > GetCommandeByRefClient(){
            
        }

        public async Task<IEnumerable<Commande> > GetCommandeByCodeExport(){
            
        }

        public async Task AddCommande(Commande commande){
            
        }

        public async Task UpdateCommande(Commande commande){
            
        }

        public async Task DeleteCommande(Commande commande){
            
        }
    }
}