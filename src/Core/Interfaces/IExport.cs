using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces{
    public interface IExportRepository{
        Task<Export> GetExportByNumAsync (int Num);

        Task<IEnumrable> GetAllExport();

        Task AddExport(Export export);

        void UpdateExprot(Export export);

        void DeleteExport(Export export);

        // autre methode 
    }
}