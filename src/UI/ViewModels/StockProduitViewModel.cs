using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Core.Services;

namespace UI.ViewModels;

public partial class StockProduitViewModel : ViewModelBase
{
    private readonly StockProduitServices _stockProduitService;

    
    public ObservableCollection<StockProduit> Inventaire { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _errorMessage;

    public StockProduitViewModel(StockProduitServices stockProduitService)
    {
        _stockProduitService = stockProduitService;
    }

    [RelayCommand]
    public async Task RefreshInventaireAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            var data = await _stockProduitService.GetAllStockDetails();
            
            Inventaire.Clear();
            foreach (var item in data)
            {
                Inventaire.Add(item);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Impossible de charger l'inventaire.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}