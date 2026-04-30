using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Core.Services;

namespace UI.ViewModels;

public partial class ProduitViewModel : ViewModelBase
{
    private readonly ProduitServices _produitService;

    public ObservableCollection<Produit> Produits { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _errorMessage = string.Empty;


    [ObservableProperty] private string _nomProduit = string.Empty;
    [ObservableProperty] private int _prix;
    [ObservableProperty] private Produit? _selectedProduit;

    public ProduitViewModel(ProduitServices produitService)
    {
        _produitService = produitService;
    }

    public ProduitViewModel() { }

   
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
            foreach (var p in data)
                Produits.Add(p);
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
    public async Task SaveProduit()
    {
        try
        {
            ErrorMessage = string.Empty;

            await _produitService.AddProduit(
                NomProduit,
                Prix
            );

            await LoadProduits();
            ResetForm();
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Erreur inattendue.";
        }
    }

    
    [RelayCommand]
    public async Task DeleteProduit(Produit? produit)
    {
        if (produit == null) return;

        try
        {
            await _produitService.DeleteProduit(produit.CodeProduit);

            Produits.Remove(produit);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    
    public void ResetForm()
    {
        NomProduit = string.Empty;
        Prix = 0;
        SelectedProduit = null;
    }
}