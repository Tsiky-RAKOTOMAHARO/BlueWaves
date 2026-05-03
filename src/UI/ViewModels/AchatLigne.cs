namespace UI.ViewModels;

public class AchatLigne
{
    public int CodeProduit  { get; set; }
    public int NumeroStock  { get; set; }
    public int Quantite     { get; set; }
    public string NomProduit { get; set; } = string.Empty;
    public string NomStock   { get; set; } = string.Empty;
}