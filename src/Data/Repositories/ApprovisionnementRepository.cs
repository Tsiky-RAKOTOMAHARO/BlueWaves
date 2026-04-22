using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories{
    public class ApprovisionnementRepository: IApprovisionnementRepository{
        
        private readonly AppDbContext _context;

        public ApprovisionnementRepository(AppDbContext context){
            _context = context;
        }
    }
}