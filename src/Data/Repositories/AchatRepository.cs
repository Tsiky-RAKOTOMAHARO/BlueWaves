using Core.Models;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories{
    public class AchatRepository: IAchatRepository{
        private readonly AppDbContext _context;

        //Constructeur
        public AchatRepository(AppDbContext context){
            _context = context;
        }

        //Les methodes d'achat
    }
}