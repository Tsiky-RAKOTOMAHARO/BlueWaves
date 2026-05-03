using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Core.Models;
using Core.Services;

namespace UI.ViewModels;

public partial class StockViewModel : ViewModelBase
{
    private readonly StockServices _stockService;
    private readonly StockProduitServices _stockProduitService;

    public ObservableCollection<Stock> Stocks { get; } = new();
    public ObservableCollection<StockProduit> ProduitsduStock { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _nomStock = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    [ObservableProperty] private Stock? _selectedStock;

    [ObservableProperty] private bool _isFormVisible;
    [ObservableProperty] private bool _isDetailVisible;

    public bool AucunProduit => ProduitsduStock.Count == 0;

    public StockViewModel(StockServices stockService, StockProduitServices stockProduitService)
    {
        _stockService = stockService;
        _stockProduitService = stockProduitService;
    }

    partial void OnSelectedStockChanged(Stock? value)
    {
        if (value != null)
            _ = LoadProduitsduStock(value.NumeroStock);
        else
            ProduitsduStock.Clear();
    }

    public async Task LoadProduitsduStock(int numStock)
    {
        try
        {
            ProduitsduStock.Clear();

            var details = await _stockProduitService.GetByStock(numStock);

            foreach (var ligne in details)
                ProduitsduStock.Add(ligne);

            OnPropertyChanged(nameof(AucunProduit));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    public async Task LoadStock()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

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

    [RelayCommand]
    public async Task SaveStock()
    {
        try
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NomStock))
            {
                ErrorMessage = "Le nom du stock est obligatoire.";
                return;
            }

            await _stockService.AddStock(NomStock);

            await LoadStock();
            ResetForm();
            IsFormVisible = false;
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    public async Task DeleteStock(Stock stock)
    {
        try
        {
            await _stockService.DeleteStock(stock);

            Stocks.Remove(stock);
            if (SelectedStock?.NumeroStock == stock.NumeroStock)
            {
                SelectedStock = null;
                ProduitsduStock.Clear();
                IsDetailVisible = false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void LoadStockForView(Stock stock)
    {
        SelectedStock = stock;
        IsDetailVisible = true;
        IsFormVisible = false;
    }

    public void OpenForm()
    {
        ResetForm();
        IsFormVisible = true;
        IsDetailVisible = false;
    }

    public void CloseForm()
    {
        IsFormVisible = false;
    }

    public void CloseDetail()
    {
        IsDetailVisible = false;
        SelectedStock = null;
        ProduitsduStock.Clear();
    }

    public void ResetForm()
    {
        NomStock = string.Empty;
        ErrorMessage = string.Empty;
    }
}