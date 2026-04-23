using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Core.Interfaces;
using Core.Models;

namespace UI.ViewModels;

public partial class StockViewModel : ViewModelBase
{
    private readonly IStockRepository _repository;

    public ObservableCollection<Stock> Stocks { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _numeroStock;
    [ObservableProperty] private string? _type;

    public StockViewModel(IStockRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Stocks.Clear();

            var data = await _repository.GetAllStock();
            
            foreach (var item in data)
            {
                Stocks.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}