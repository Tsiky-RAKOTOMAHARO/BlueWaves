using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Core.Models{
    public class Stock{

        [Key]
        public int NumeroStock {get; set;}
        
        [Required]
        [StringLength(25)]
        public string NomStock { get; set; } = string.Empty;

        public virtual ICollection<Approvisionnement> Approvisionnements { get; set; }
        = new List<Approvisionnement>();

        public virtual ICollection<StockProduit> StockProduits { get; set; }  // ← à ajouter
            = new List<StockProduit>();
    }
}