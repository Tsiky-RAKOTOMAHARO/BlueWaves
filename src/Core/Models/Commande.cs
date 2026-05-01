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
        public int NumeroExport { get; set; } // ✅ corrigé

        [Required]
        public DateTime DateCommande { get; set; }

        [Required]
        [StringLength(150)]
        public string Destination { get; set; } = string.Empty; // ✅ safe

        // Relations
        [ForeignKey(nameof(RefClient))]
        public virtual Client Client { get; set; } = null!;

        [ForeignKey(nameof(NumeroExport))]
        public virtual Export Export { get; set; } = null!;

        // 🔥 IMPORTANT : relation avec ACHAT
        public virtual ICollection<Achat> Achats { get; set; }
            = new List<Achat>();
    }
}