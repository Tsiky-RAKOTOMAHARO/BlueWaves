using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories{
    public class ExportRepository: IExportRepository{
        private readonly AppDbContext _context;

        public ExportRepository(AppDbContext context){
            _context = context;
        }

        public async Task<Export> GetExportByNumAsync (int Num){
            
        }

        public async Task<IEnumerable<Export>> GetAllExport(){
            
        }

        public async Task AddExport(Export export){
            
        }

        public async Task UpdateExprot(Export export){
            
        }

        public async Task DeleteExport(Export export){
            
        }
    }
}