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

    // Collection pour la liste des produits
    public ObservableCollection<Produit> Produits { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // Propriétés du formulaire
    [ObservableProperty] private string _nomProduit = string.Empty;
    [ObservableProperty] private int _quantite;
    [ObservableProperty] private DateTime _dateReception = DateTime.Now;
    [ObservableProperty] private int _numeroStock;
    [ObservableProperty] private Produit? _selectedProduit;

    public ProduitViewModel(ProduitServices produitService)
    {
        _produitService = produitService;
    }

    // Constructeur vide pour le designer XAML si nécessaire
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
            foreach (var item in data)
            {
                Produits.Add(item);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erreur lors du chargement : " + ex.Message;
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

            // Appel au service (qui contient les validations métier)
            await _produitService.AddProduit(NomProduit, Quantite, DateReception, NumeroStock);
            
            // Rafraîchir la liste et réinitialiser le formulaire
            await LoadProduits();
            ResetForm();
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Une erreur inattendue est survenue.";
        }
    }

    [RelayCommand]
    public async Task DeleteProduit(Produit? produit)
    {
        if (produit == null) return;

        try
        {
            await _produitService.DeleteProduit(produit);
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
        Quantite = 0;
        DateReception = DateTime.Now;
        NumeroStock = 0;
        ErrorMessage = string.Empty;
    }
}