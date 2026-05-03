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

        if (_stockVM != null)
            await _stockVM.LoadStockCommand.ExecuteAsync(null);
    }

    private async void SaveStock_Click(object? sender, RoutedEventArgs e)
    {
        if (_stockVM == null) return;

        await _stockVM.SaveStockCommand.ExecuteAsync(null);

        if (string.IsNullOrEmpty(_stockVM.ErrorMessage))
            _stockVM.IsFormVisible = false;
    }

    private void OuvrirPanneau_Click(object? sender, RoutedEventArgs e)
    {
        if (_stockVM == null) return;

        _stockVM.OpenForm();
    }

    private void FermerPanneau_Click(object? sender, RoutedEventArgs e)
    {
        if (_stockVM == null) return;

        _stockVM.CloseForm();
    }

    private void OnStockSelected_Click(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is Stock stock && _stockVM != null)
        {
            _stockVM.LoadStockForView(stock);
        }
    }

    private void FermerDetail_Click(object? sender, RoutedEventArgs e)
    {
        if (_stockVM == null) return;

        _stockVM.CloseDetail();
    }
}