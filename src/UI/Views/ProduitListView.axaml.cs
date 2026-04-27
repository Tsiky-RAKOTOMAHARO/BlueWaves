using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Core.Models;
using Core.Services;

namespace UI.Views;

public partial class ProduitListView : UserControl{
    private ProduitServices? _produitServices;
    private ObservableCollection<Produit> _produits = new();

    public ProduitListView(){
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            _produitServices = Program.ServiceHost.Services.GetRequiredService<ProduitServices>();
        }
    }

    protected override async void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e){
        base.OnAttachedToVisualTree(e);

        // Récupération et abonnement manuels des boutons
        if (this.FindControl<Button>("AjouterButton") is Button btnAjouter)
        {
            btnAjouter.Click -= OuvrirPanneau_Click;
            btnAjouter.Click += OuvrirPanneau_Click;
        }

        if (this.FindControl<Button>("FermerPanneauButton") is Button btnFermer)
        {
            btnFermer.Click -= FermerPanneau_Click;
            btnFermer.Click += FermerPanneau_Click;
        }

        if (this.FindControl<Button>("AnnulerButton") is Button btnAnnuler)
        {
            btnAnnuler.Click -= FermerPanneau_Click;
            btnAnnuler.Click += FermerPanneau_Click;
        }

        if (this.FindControl<Button>("EnregistrerButton") is Button btnEnregistrer)
        {
            btnEnregistrer.Click -= Enregistrer_Click;
            btnEnregistrer.Click += Enregistrer_Click;
        }

        await ChargerProduits();
    }

    private void OuvrirPanneau_Click(object? sender, RoutedEventArgs e) => OuvrirPanneau();
    private void FermerPanneau_Click(object? sender, RoutedEventArgs e) => FermerPanneau();

    private void OuvrirPanneau()
    {
        // Réinitialisation manuelle via FindControl
        if (this.FindControl<TextBox>("NomProduitInput") is TextBox nomInput) nomInput.Text = string.Empty;
        if (this.FindControl<TextBox>("NumeroStockInput") is TextBox stockInput) stockInput.Text = string.Empty;
        if (this.FindControl<TextBox>("QuantiteInput") is TextBox qteInput) qteInput.Text = string.Empty;
        // On change <DatePicker> par <CalendarDatePicker>
if (this.FindControl<CalendarDatePicker>("DateReceptionInput") is CalendarDatePicker dateInput) 
{
    // Attention : CalendarDatePicker utilise DateTime?, pas DateTimeOffset?
    dateInput.SelectedDate = DateTime.Now; 
}
        if (this.FindControl<TextBlock>("ErreurMessage") is TextBlock erreurMsg) erreurMsg.IsVisible = false;

        if (this.FindControl<Border>("PanneauLateral") is Border panneau) panneau.IsVisible = true;
    }

    private void FermerPanneau()
    {
        if (this.FindControl<Border>("PanneauLateral") is Border panneau) panneau.IsVisible = false;
    }

    private async void Enregistrer_Click(object? sender, RoutedEventArgs e)
{
    if (_produitServices == null) return;

    // Récupération des contrôles
    var nomInput = this.FindControl<TextBox>("NomProduitInput");
    var stockInput = this.FindControl<TextBox>("NumeroStockInput");
    var qteInput = this.FindControl<TextBox>("QuantiteInput");
    

    var dateInput = this.FindControl<CalendarDatePicker>("DateReceptionInput");

    // Validation
    if (nomInput == null || string.IsNullOrWhiteSpace(nomInput.Text))
    {
        AfficherErreur("Le nom du produit est obligatoire.");
        return;
    }

    if (stockInput == null || !int.TryParse(stockInput.Text, out int numeroStock))
    {
        AfficherErreur("Le numéro de stock doit être un entier.");
        return;
    }

    if (qteInput == null || !int.TryParse(qteInput.Text, out int quantite))
    {
        AfficherErreur("La quantité doit être un entier.");
        return;
    }

  
    if (dateInput == null || dateInput.SelectedDate is null)
    {
        AfficherErreur("La date de réception est obligatoire.");
        return;
    }

    try
    {
        var produit = new Produit
        {
            NomProduit = nomInput.Text.Trim(),
            NumeroStock = numeroStock,
            Quantite = quantite,
            // .SelectedDate.Value pour un CalendarDatePicker est déjà un DateTime
            Date_reception = dateInput.SelectedDate.Value, 
            Statut = true
        };

        await _produitServices.AddProduit(produit);
        FermerPanneau();
        await ChargerProduits();
    }
    catch (Exception ex)
    {
        AfficherErreur($"Erreur lors de l'enregistrement : {ex.Message}");
        Debug.WriteLine($"[ProduitListView] Erreur sauvegarde : {ex.Message}");
    }
}

    private void AfficherErreur(string message)
    {
        if (this.FindControl<TextBlock>("ErreurMessage") is TextBlock erreurMsg)
        {
            erreurMsg.Text = message;
            erreurMsg.IsVisible = true;
        }
    }

    private async Task ChargerProduits()
    {
        if (_produitServices == null) return;

        try
        {
            var produits = await _produitServices.GetAllProduit();
            _produits = new ObservableCollection<Produit>(produits);
            
            if (this.FindControl<DataGrid>("ProductGrid") is DataGrid grid)
            {
                grid.ItemsSource = _produits;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ProduitListView] Erreur chargement produits : {ex.Message}");
        }
    }
}