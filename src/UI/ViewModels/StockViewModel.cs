using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Core.Models;
using Core.Services;

namespace UI.ViewModels;

public partial class StockViewModel : ViewModelBase
{
    private readonly StockServices _stockService;

    public ObservableCollection<Stock> Stocks { get; } = new();
    public ObservableCollection<Produit> ProduitsduStock { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _nomStock = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private Stock? _selectedStock;

    public bool AucunProduit => ProduitsduStock.Count == 0;

    public StockViewModel(StockServices stockService)
    {
        _stockService = stockService;
    }

    partial void OnSelectedStockChanged(Stock? value)
    {
        if (value != null)
            _ = LoadProduitsduStock(value.NumeroStock);
        else
            ProduitsduStock.Clear();
    }

    [RelayCommand]
    public async Task LoadStock()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;

            Stocks.Clear();
            var data = await _stockService.GetAllStock();

            foreach (var item in data)
                Stocks.Add(item);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadProduitsduStock(int numStock)
    {
        try
        {
            ProduitsduStock.Clear();

            var stock = await _stockService.GetStockByNum(numStock);

            if (stock == null)
                return;

            foreach (var appro in stock.Approvisionnements)
            {
                ProduitsduStock.Add(appro.Produit);
            }

            OnPropertyChanged(nameof(AucunProduit));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    public async Task SaveStock()
    {
        try
        {
            ErrorMessage = string.Empty;

            await _stockService.AddStock(NomStock);
            await LoadStock();
            ResetForm();
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task DeleteStock(Stock stock)
    {
        try
        {
            await _stockService.DeleteStock(stock);
            Stocks.Remove(stock);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void ResetForm()
    {
        NomStock = string.Empty;
        ErrorMessage = string.Empty;
    }
}