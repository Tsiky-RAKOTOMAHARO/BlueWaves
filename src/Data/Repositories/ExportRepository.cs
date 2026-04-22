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
    }
}