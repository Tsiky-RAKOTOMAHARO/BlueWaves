using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Core.Models;
using Core.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class ApprovisionnementViewModel : ViewModelBase
{
    private readonly ApprovisionnementServices _approService;
    private readonly ProduitServices           _produitService;
    private readonly FournisseurServices       _fournisseurService;
    private readonly StockServices             _stockService;

    public ObservableCollection<Approvisionnement> Approvisionnements         { get; } = new();
    public ObservableCollection<Approvisionnement> FilteredApprovisionnements { get; } = new();
    public ObservableCollection<Produit>           Produits                   { get; } = new();
    public ObservableCollection<Fournisseur>       Fournisseurs               { get; } = new();
    public ObservableCollection<Stock>             Stocks                     { get; } = new();

    // ── État UI ───────────────────────────────────────────────
    [ObservableProperty] private bool    _isLoading;
    [ObservableProperty] private bool    _isFormVisible;      // ← AJOUT : remplace panel.IsVisible dans le View
    [ObservableProperty] private string  _errorMessage = string.Empty;
    [ObservableProperty] private string? _searchQuery;

    // ── Champs du formulaire ──────────────────────────────────
    [ObservableProperty] private string?      _certificatSaisi = string.Empty;
    [ObservableProperty] private int          _quantiteSaisie;
    [ObservableProperty] private Fournisseur? _selectedFournisseur;
    [ObservableProperty] private Produit?     _selectedProduit;
    [ObservableProperty] private Stock?       _selectedStock;

    public ApprovisionnementViewModel(
        ApprovisionnementServices approService,
        ProduitServices           produitService,
        FournisseurServices       fournisseurService,
        StockServices             stockService)
    {
        _approService       = approService;
        _produitService     = produitService;
        _fournisseurService = fournisseurService;
        _stockService       = stockService;
    }

    // ── Commandes (remplacent tous les handlers du View) ──────

    [RelayCommand]
    private void NouvelleReception()
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

    [RelayCommand]                              // ← était public async Task appelé depuis View
    private async Task Sauvegarder()
    {
        if (string.IsNullOrWhiteSpace(CertificatSaisi) ||
            SelectedFournisseur is null ||
            SelectedProduit     is null ||
            SelectedStock       is null ||
            QuantiteSaisie <= 0)
        {
            ErrorMessage = "Veuillez remplir correctement tous les champs.";
            return;
        }

        try
        {
            var appro = new Approvisionnement
            {
                CodeProduit    = SelectedProduit.CodeProduit,
                RefFournisseur = SelectedFournisseur.RefFournisseur,
                NumeroStock    = SelectedStock.NumeroStock,
                Certificat     = CertificatSaisi,
                Quantite       = QuantiteSaisie,
                DateReception  = DateTime.Now,
            };

            await _approService.AddApprovisionnement(appro);
            await LoadDataAsync();

            ResetForm();
            IsFormVisible = false;
        }
        catch
        {
            ErrorMessage = "Erreur lors de l'ajout.";
        }
    }

    [RelayCommand]
    private async Task Supprimer(Approvisionnement appro)
    {
        if (appro is null) return;
        try
        {
            IsLoading = true;
            await _approService.DeleteApprovisionnement(appro);
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

    // ── Chargement ────────────────────────────────────────────

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            var appros  = await _approService.GetAllApprovisionnement();
            var prods   = await _produitService.GetAllProduit();
            var fours   = await _fournisseurService.GetAllFournisseur();
            var stocks  = await _stockService.GetAllStock();

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Approvisionnements.Clear();
                FilteredApprovisionnements.Clear();
                foreach (var a in appros)
                {
                    Approvisionnements.Add(a);
                    FilteredApprovisionnements.Add(a);
                }

                Produits.Clear();
                foreach (var p in prods)   Produits.Add(p);

                Fournisseurs.Clear();
                foreach (var f in fours)   Fournisseurs.Add(f);

                Stocks.Clear();
                foreach (var s in stocks)  Stocks.Add(s);
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erreur de chargement des données.";
            Debug.WriteLine(ex);         // ← Debug.WriteLine au lieu de Console, retiré en release
        }
        finally
        {
            IsLoading = false;
        }
    }

    // ── Helpers privés ────────────────────────────────────────

    private void ResetForm()
    {
        CertificatSaisi     = string.Empty;
        QuantiteSaisie      = 0;
        SelectedFournisseur = null;
        SelectedProduit     = null;
        SelectedStock       = null;
        ErrorMessage        = string.Empty;
    }

    partial void OnSearchQueryChanged(string? value)
    {
        var query   = value ?? string.Empty;
        var results = string.IsNullOrWhiteSpace(query)
            ? Approvisionnements
            : Approvisionnements.Where(a =>
                (a.Certificat?.Contains(query, StringComparison.OrdinalIgnoreCase)               ?? false) ||
                (a.Fournisseur?.NomFournisseur?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));

        FilteredApprovisionnements.Clear();
        foreach (var item in results)
            FilteredApprovisionnements.Add(item);
    }
}