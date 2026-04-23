using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces{
    public interface IExportRepository{
        Task<Export> GetExportByNum (int Num);

        Task<IEnumerable<Export>> GetAllExport();

        Task AddExport(Export export);

        Task UpdateExport(Export export);

        Task DeleteExport(Export export);

        // autre methode 
    }
}