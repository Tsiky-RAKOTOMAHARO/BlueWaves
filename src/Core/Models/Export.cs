using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Core.Models{
    public class Export{

        [Key]
        public int NumeroExport {get; set;}

        [Required]
        public int Delai{get; set;}

        public virtual ICollection<Commande> Commande {get; set;} = new List<Commande>();
    }
}