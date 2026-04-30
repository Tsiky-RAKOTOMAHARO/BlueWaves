using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using UI.ViewModels;

namespace UI.Views;

public partial class ProduitListView : UserControl
{
    private ProduitViewModel? _vm;

    public ProduitListView()
    {
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            _vm = Program.ServiceHost.Services.GetRequiredService<ProduitViewModel>();
            DataContext = _vm;
        }
    }

    protected override async void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (_vm != null && _vm.Produits.Count == 0)
            await _vm.LoadProduits();
    }

    private void OuvrirPanneau_Click(object? sender, RoutedEventArgs e)
    {
        if (_vm == null) return;

        _vm.ResetForm();
        PanneauLateral.IsVisible = true;
    }

    private void FermerPanneau_Click(object? sender, RoutedEventArgs e)
    {
        PanneauLateral.IsVisible = false;
    }

    private void OnProduitSearchChanged(object? sender, TextChangedEventArgs e)
    {
    }

    private void OnProduitActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Produit produit)
            Debug.WriteLine($"Actions pour : {produit.NomProduit}");
    }
}