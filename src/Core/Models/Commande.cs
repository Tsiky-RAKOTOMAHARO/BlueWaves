using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Core.Models
{
    public class Commande
    {
        [Key]
        public int NumeroCommande { get; set; }

        [Required]
        public int RefClient { get; set; }

        [Required]
        public DateTime DateCommande { get; set; }

        [Required]
        [StringLength(150)]
        public string Destination { get; set; } = string.Empty; 

        [Required]
        public int Delai { get; set; }

        // Relations
        [ForeignKey(nameof(RefClient))]
        public virtual Client Client { get; set; } = null!;

        public virtual ICollection<Achat> Achats { get; set; }
            = new List<Achat>();
    }
}