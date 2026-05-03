using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Core.Services;

namespace UI.ViewModels;

public partial class ProduitViewModel : ViewModelBase
{
    private readonly ProduitServices _produitService;

    public ObservableCollection<Produit> Produits         { get; } = new();
    public ObservableCollection<Produit> FilteredProduits { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private bool _isFormVisible;

    [ObservableProperty] private string _searchQuery;

    [ObservableProperty] private string _nomProduit = string.Empty;
    [ObservableProperty] private int _prix;
    [ObservableProperty] private Produit? _selectedProduit;

    public ProduitViewModel(ProduitServices produitService)
    {
        _produitService = produitService;
    }

    [RelayCommand]
    private void NouveauProduit()
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
    public async Task LoadProduits()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var data = await _produitService.GetAllProduit();

            Produits.Clear();
            FilteredProduits.Clear();

            foreach (var p in data)
            {
                Produits.Add(p);
                FilteredProduits.Add(p);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erreur chargement produits : " + ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task Sauvegarder()
    {
        if (string.IsNullOrWhiteSpace(NomProduit))
        {
            ErrorMessage = "Le nom du produit est obligatoire.";
            return;
        }

        try
        {
            ErrorMessage = string.Empty;

            if (SelectedProduit is null)
            {
                await _produitService.AddProduit(NomProduit, Prix);
            }
            else
            {
                SelectedProduit.NomProduit = NomProduit;
                SelectedProduit.Prix = Prix;
                await _produitService.UpdateProduit(SelectedProduit);
            }

            await LoadProduits();
            ResetForm();
            IsFormVisible = false;
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erreur lors de l'enregistrement : " + ex.Message;
        }
    }

    public void LoadProduitForEdit(Produit produit)
    {
        SelectedProduit = produit;
        NomProduit = produit.NomProduit;
        Prix = produit.Prix;
        IsFormVisible = true;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    public async Task Supprimer(Produit? produit)
    {
        if (produit == null) return;

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            await _produitService.DeleteProduit(produit.CodeProduit);

            Produits.Remove(produit);
            FilteredProduits.Remove(produit);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnSearchQueryChanged(string value)
    {
        var query = value ?? string.Empty;

        var result = string.IsNullOrWhiteSpace(query)
            ? Produits
            : Produits.Where(p =>
                (p.NomProduit?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));

        FilteredProduits.Clear();
        foreach (var p in result)
            FilteredProduits.Add(p);
    }

    public void ResetForm()
    {
        NomProduit = string.Empty;
        Prix = 0;
        SelectedProduit = null;
        ErrorMessage = string.Empty;
    }
}