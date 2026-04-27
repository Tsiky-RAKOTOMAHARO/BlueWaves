using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Core.Models{
    public class Fournisseur{
        [Key]
        public int RefFournisseur {get; set;}

        [Required]
        [StringLength(150)]
        public string NomFournisseur {get; set;}

        [StringLength(150)]
        public string PrenomFournisseur {get; set;}

        [Required]
        [StringLength(25)]
        public string TelephoneFournisseur {get; set;}


        public virtual ICollection<Approvisionnement> Approvisionnement {get; set;} = new List<Approvisionnement>();
    }
}