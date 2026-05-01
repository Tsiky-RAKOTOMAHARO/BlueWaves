using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class StockProduit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NumeroStock { get; set; }
        
        [ForeignKey("NumeroStock")]
        public virtual Stock? Stock { get; set; }

        [Required]
        public int CodeProduit { get; set; } 
        
        [ForeignKey("CodeProduit")]
        public virtual Produit? Produit { get; set; }

        public int Quantite { get; set; } 
    }
}