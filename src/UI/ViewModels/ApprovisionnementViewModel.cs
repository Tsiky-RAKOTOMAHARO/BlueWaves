using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace UI.ViewModels;

public partial class ApprovisionnementViewModel : ViewModelBase
{
    private readonly ApprovisionnementServices _approService;
    private readonly ProduitServices _produitService;
    private readonly FournisseurServices _fournisseurService;
    private readonly StockServices _stockService;

    public ObservableCollection<Approvisionnement> Approvisionnements { get; } = new();
    public ObservableCollection<Approvisionnement> FilteredApprovisionnements { get; } = new();
    public ObservableCollection<Produit> Produits { get; } = new();
    public ObservableCollection<Fournisseur> Fournisseurs { get; } = new();
    public ObservableCollection<Stock> Stocks { get; } = new();

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? searchQuery;
    [ObservableProperty] private string errorMessage = string.Empty;

    [ObservableProperty] private Produit? selectedProduit;
    [ObservableProperty] private Fournisseur? selectedFournisseur;
    [ObservableProperty] private Stock? selectedStock;
    [ObservableProperty] private int quantiteSaisie;
    [ObservableProperty] private string certificatSaisi = string.Empty;

    public ApprovisionnementViewModel(
        ApprovisionnementServices approService,
        ProduitServices produitService,
        FournisseurServices fournisseurService,
        StockServices stockService)
    {
        _approService = approService;
        _produitService = produitService;
        _fournisseurService = fournisseurService;
        _stockService = stockService;
    }

       public async Task LoadDataAsync()
{
    if (IsLoading) return;

    try
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        var appros = await _approService.GetAllApprovisionnement();
        var prods = await _produitService.GetAllProduit();
        var fours = await _fournisseurService.GetAllFournisseur();
        var stocksData = await _stockService.GetAllStock();

        Console.WriteLine($"Appros count: {appros.Count()}");
        foreach (var a in appros)
            Console.WriteLine($"Certificat: {a.Certificat} | Fournisseur: {a.Fournisseur?.NomFournisseur ?? "NULL"}");

        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            Approvisionnements.Clear();
            FilteredApprovisionnements.Clear();
            foreach (var item in appros)
            {
                Approvisionnements.Add(item);
                FilteredApprovisionnements.Add(item);
            }

            Produits.Clear();
            foreach (var p in prods) Produits.Add(p);

            Fournisseurs.Clear();
            foreach (var f in fours) Fournisseurs.Add(f);

            Stocks.Clear();
            foreach (var s in stocksData) Stocks.Add(s);
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERREUR LoadDataAsync: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
        ErrorMessage = "Erreur de chargement des données.";
    }
    finally
    {
        IsLoading = false;
    }
} 
   
    public async Task AddApprovisionnementAsync()
    {
        try
        {
            if (SelectedProduit == null || SelectedFournisseur == null || SelectedStock == null)
            {
                ErrorMessage = "Champs obligatoires";
                return;
            }

            if (QuantiteSaisie <= 0)
            {
                ErrorMessage = "Quantité invalide";
                return;
            }

            var nouveauAppro = new Approvisionnement
            {
                CodeProduit = SelectedProduit.CodeProduit,
                RefFournisseur = SelectedFournisseur.RefFournisseur,
                NumeroStock = SelectedStock.NumeroStock,
                Certificat = CertificatSaisi,
                Quantite = QuantiteSaisie,
                DateReception = DateTime.Now
            };

            await _approService.AddApprovisionnement(nouveauAppro);

            ResetForm();
        }
        catch
        {
            ErrorMessage = "Erreur lors de l'ajout.";
        }
    }

    public void ResetForm()
    {
        SelectedProduit = null;
        SelectedFournisseur = null;
        SelectedStock = null;
        QuantiteSaisie = 0;
        CertificatSaisi = string.Empty;
        ErrorMessage = string.Empty;
    }

    partial void OnSearchQueryChanged(string? value)
    {
        FilteredApprovisionnements.Clear();

        var query = value ?? "";

        var results = string.IsNullOrWhiteSpace(query)
            ? Approvisionnements
            : Approvisionnements.Where(a =>
                (a.Certificat?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (a.Fournisseur?.NomFournisseur?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
            );

        foreach (var item in results)
            FilteredApprovisionnements.Add(item);
    }

    public async Task DeleteApprovisionnementAsync(Approvisionnement approvisionnement)
    {
        if (approvisionnement == null) return;

        try
        {
            IsLoading = true;

            await _approService.DeleteApprovisionnement(approvisionnement);

            await LoadDataAsync();
        }
        catch
        {
            ErrorMessage = "Erreur suppression.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}