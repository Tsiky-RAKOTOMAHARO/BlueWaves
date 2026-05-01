using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Core.Models;
using UI.ViewModels;

namespace UI.Views;

public partial class StockInventoryView : UserControl
{
    private StockViewModel? _stockVM;
    private ApprovisionnementViewModel? _approVM;
    


    public StockInventoryView()
    {
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            _stockVM = Program.ServiceHost.Services.GetRequiredService<StockViewModel>();
            DataContext = _stockVM;
        }
    }

    protected override async void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (_stockVM != null && _stockVM.Stocks.Count == 0)
            await _stockVM.LoadStockCommand.ExecuteAsync(null);
    }

    private void OuvrirPanneau_Click(object? sender, RoutedEventArgs e)
    {
        _stockVM?.ResetForm();
        PanneauDetail.IsVisible  = false;
        PanneauLateral.IsVisible = true;
    }

    private void FermerPanneau_Click(object? sender, RoutedEventArgs e)
        => PanneauLateral.IsVisible = false;

    private async void SaveStock_Click(object? sender, RoutedEventArgs e)
    {
        if (_stockVM == null) return;
        await _stockVM.SaveStockCommand.ExecuteAsync(null);
        if (string.IsNullOrEmpty(_stockVM.ErrorMessage))
            PanneauLateral.IsVisible = false;
    }

    private void OnStockSelected_Click(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is Stock stock)
        {
            if (_stockVM != null) _stockVM.SelectedStock = stock;
            PanneauLateral.IsVisible = false;
            PanneauDetail.IsVisible  = true;
        }
    }

    private void FermerDetail_Click(object? sender, RoutedEventArgs e)
    {
        PanneauDetail.IsVisible = false;
        if (_stockVM != null) _stockVM.SelectedStock = null;
    }
}