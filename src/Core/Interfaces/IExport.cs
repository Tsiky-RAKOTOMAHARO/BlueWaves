using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces{
    public interface IExportRepository{
        Task<Export> GetExportByNumAsync (int Num);

        Task<IEnumerable<Export>> GetAllExport();

        Task AddExport(Export export);

        Task UpdateExprot(Export export);

        Task DeleteExport(Export export);

        // autre methode 
    }
}