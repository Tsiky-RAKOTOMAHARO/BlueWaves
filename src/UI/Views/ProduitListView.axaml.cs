using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using UI.ViewModels;

namespace UI.Views;

public partial class ProduitListView : UserControl
{
    private ProduitViewModel? _produitVM;

    public ProduitListView()
    {
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            _produitVM = Program.ServiceHost.Services.GetRequiredService<ProduitViewModel>();
            DataContext = _produitVM;
        }
    }

    protected override async void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (_produitVM != null && _produitVM.Produits.Count == 0)
            await _produitVM.LoadProduits();
    }

    private async void DeleteProduit_Click(object? sender, RoutedEventArgs e)
    {
    if (sender is MenuItem mi && mi.Tag is Produit produit && _produitVM != null)
    {
        await _produitVM.DeleteProduit(produit);
    }
    }
    private void EditProduit_Click(object? sender, RoutedEventArgs e)
    {
    if (sender is MenuItem mi && mi.Tag is Produit produit && _produitVM != null)
    {
        _produitVM.LoadProduitForEdit(produit);
        PanneauLateral.IsVisible = true;
    }
    }
    private void OuvrirPanneau_Click(object? sender, RoutedEventArgs e)
    {
        if (_produitVM == null) return;

        _produitVM.ResetForm();
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