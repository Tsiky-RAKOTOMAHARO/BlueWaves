using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace UI.ViewModels;

public partial class CommandeViewModel : ViewModelBase
{
    private readonly CommandeServices _commandeService;
    private readonly AchatServices    _achatService;
    private readonly ClientServices   _clientService;
    private readonly ProduitServices  _produitService;
    private readonly StockServices    _stockService;
   
    public ObservableCollection<Commande>   Commandes         { get; } = new();
    public ObservableCollection<Commande>   FilteredCommandes { get; } = new();
    public ObservableCollection<AchatLigne> Lignes            { get; } = new();

    
    public ObservableCollection<Client>  Clients  { get; } = new();
    public ObservableCollection<Produit> Produits { get; } = new();
    public ObservableCollection<Stock>   Stocks   { get; } = new();

   
    [ObservableProperty] private bool    _isLoading;
    [ObservableProperty] private bool    _isFormVisible;       
    [ObservableProperty] private string  _errorMessage = string.Empty;
    [ObservableProperty] private string? _searchQuery;


    [ObservableProperty] private string   _destination  = string.Empty;
    [ObservableProperty] private DateTime _dateCommande = DateTime.Now;
    [ObservableProperty] private int      _delai;
    [ObservableProperty] private Client?  _selectedClient;     

  
    [ObservableProperty] private Produit? _selectedProduit;    
    [ObservableProperty] private int      _quantite;

    [ObservableProperty] private Commande? _selectedCommande;
    [ObservableProperty] private Stock?   _selectedStock;

    public CommandeViewModel(
        CommandeServices commandeService,
        AchatServices    achatService,
        ClientServices   clientService,
        ProduitServices  produitService,
        StockServices    stockService)
    {
        _commandeService = commandeService;
        _achatService    = achatService;
        _clientService   = clientService;
        _produitService  = produitService;
        _stockService    = stockService;
    }


    [RelayCommand]
    private void NouvelleCommande()
    {
        ResetForm();
        IsFormVisible = true;
    }

    [RelayCommand]
    private void FermerFormulaire()
    {
        ResetForm();
        IsFormVisible = false;
    }

    [RelayCommand]
    private void AjouterLigne()
    {
        if (SelectedProduit is null || SelectedStock is null || Quantite <= 0)
        {
            ErrorMessage = "Produit, stock et quantité obligatoires.";
            return;
        }

        Lignes.Add(new AchatLigne
        {
            CodeProduit = SelectedProduit.CodeProduit,
            NumeroStock = SelectedStock.NumeroStock,
            Quantite    = Quantite
        });

        SelectedProduit = null;
        SelectedStock   = null;
        Quantite        = 0;
        ErrorMessage    = string.Empty;
    }

    [RelayCommand]
    private void RetirerLigne(AchatLigne ligne)
        => Lignes.Remove(ligne);

    [RelayCommand]
    private async Task ConfirmerCommande()
    {
        if (string.IsNullOrWhiteSpace(Destination) ||
            SelectedClient is null ||
            Delai <= 0)
        {
            ErrorMessage = "Veuillez remplir correctement tous les champs.";
            return;
        }

        if (!Lignes.Any())
        {
            ErrorMessage = "Ajoutez au moins une ligne.";
            return;
        }

        try
        {
            ErrorMessage = string.Empty;

            var commande = await _commandeService.AddCommande(
                Destination, DateCommande, Delai, SelectedClient.RefClient);

            foreach (var ligne in Lignes)
            {
                await _achatService.AddAchat(new Achat
                {
                    NumeroCommande = commande.NumeroCommande,
                    CodeProduit    = ligne.CodeProduit,
                    NumeroStock    = ligne.NumeroStock,
                    Quantite       = ligne.Quantite
                });
            }

            await LoadDataAsync();
            ResetForm();
            IsFormVisible = false;
        }
        catch (ArgumentException ex)        { ErrorMessage = ex.Message; }
        catch (InvalidOperationException ex) { ErrorMessage = ex.Message; }
    }

    [RelayCommand]
    private async Task UpdateCommande()
    {
        if (SelectedCommande is null)
        {
            ErrorMessage = "Aucune commande sélectionnée.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Destination) || SelectedClient is null || Delai <= 0)
        {
            ErrorMessage = "Veuillez remplir correctement tous les champs.";
            return;
        }

        try
        {
            SelectedCommande.Destination  = Destination;
            SelectedCommande.DateCommande = DateCommande;
            SelectedCommande.Delai        = Delai;
            SelectedCommande.RefClient    = SelectedClient.RefClient;

            await _commandeService.UpdateCommande(SelectedCommande);
            await LoadDataAsync();

            ResetForm();
            IsFormVisible    = false;
            SelectedCommande = null;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private async Task DeleteCommande(Commande commande)
    {
        try
        {
            await _commandeService.DeleteCommande(commande);
            Commandes.Remove(commande);
            FilterCommandes(SearchQuery ?? string.Empty);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }


    public async Task LoadDataAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            var commandes = await _commandeService.GetAllCommande();
            var clients   = await _clientService.GetAllClient();
            var produits  = await _produitService.GetAllProduit();
            var stocks    = await _stockService.GetAllStock();

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Commandes.Clear();
                FilteredCommandes.Clear();
                foreach (var c in commandes)
                {
                    Commandes.Add(c);
                    FilteredCommandes.Add(c);
                }

                Clients.Clear();
                foreach (var c in clients)  Clients.Add(c);

                Produits.Clear();
                foreach (var p in produits) Produits.Add(p);

                Stocks.Clear();
                foreach (var s in stocks)   Stocks.Add(s);
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erreur de chargement des données.";
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }


    private void ResetForm()
    {
        Destination     = string.Empty;
        DateCommande    = DateTime.Now;
        Delai           = 0;
        SelectedClient  = null;
        SelectedProduit = null;
        SelectedStock   = null;
        Quantite        = 0;
        Lignes.Clear();
        ErrorMessage    = string.Empty;
    }

    partial void OnSearchQueryChanged(string? value)
        => FilterCommandes(value ?? string.Empty);

    private void FilterCommandes(string query)
    {
        var results = string.IsNullOrWhiteSpace(query)
            ? Commandes
            : Commandes.Where(c =>
                (c.Destination?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                c.NumeroCommande.ToString().Contains(query));

        FilteredCommandes.Clear();
        foreach (var item in results)
            FilteredCommandes.Add(item);
    }
}