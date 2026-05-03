using Avalonia.Controls;
using Avalonia.Interactivity;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using UI.ViewModels;

namespace UI.Views;

public partial class ProduitListView : UserControl
{
    public ProduitViewModel ProduitVM { get; }

    public ProduitListView()
    {
        var services = Program.ServiceHost!.Services;
        ProduitVM = services.GetRequiredService<ProduitViewModel>();

        DataContext = this;
        InitializeComponent();
    }

    protected override async void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        await ProduitVM.LoadProduits();
    }

    private void OnProduitActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            btn.ContextMenu?.Open(btn);
        }
    }

    private void OnEditProduitClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: Produit p })
        {
            ProduitVM.LoadProduitForEdit(p);
        }
    }

    private void OnDeleteProduitClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: Produit p })
        {
            ProduitVM.SupprimerCommand.Execute(p);
        }
    }

    private void OnNouveauProduitClick(object? sender, RoutedEventArgs e)
    {
        ProduitVM.NouveauProduitCommand.Execute(null);
    }

    private void OnFermerFormulaireClick(object? sender, RoutedEventArgs e)
    {
        ProduitVM.FermerFormulaireCommand.Execute(null);
    }
}