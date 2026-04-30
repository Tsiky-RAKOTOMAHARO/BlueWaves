using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace Core.Models{
public class Produit
{
    [Key]
    public int CodeProduit { get; set; }

    [Required]
    [StringLength(150)]
    public string NomProduit { get; set; }

    [Required]
    public int Prix { get; set; }

    public bool Statut { get; set; }

    public virtual ICollection<Approvisionnement> Approvisionnements { get; set; }
        = new List<Approvisionnement>();
}
}