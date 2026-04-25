using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Interfaces;

namespace Core.Services{
    public class ExportServices : IExportRepository{
        private readonly IExportRepository _exportRepository;

        public ExportServices(IExportRepository exportRepository){
            _exportRepository = exportRepository;
        }

        public async Task<Export?> GetExportByNum(int num){
            return await _exportRepository.GetExportByNum(num);
        }

        public async Task<IEnumerable<Export>> GetAllExport(){
            return await _exportRepository.GetAllExport();
        }

        public async Task AddExport(Export export){
            await _exportRepository.AddExport(export);
        }

        public async Task UpdateExport(Export export){
            await _exportRepository.UpdateExport(export);
        }

        public async Task DeleteExport(Export export){
            await _exportRepository.DeleteExport(export);
        }
    }
}