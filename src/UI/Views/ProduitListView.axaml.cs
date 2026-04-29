using Avalonia.Controls;
using Avalonia.Interactivity;
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
            await _vm.LoadProduitsCommand.ExecuteAsync(null);
    }

    private void OuvrirPanneau_Click(object? sender, RoutedEventArgs e)
    {
        _vm?.ResetForm();
        PanneauLateral.IsVisible = true;
    }

    private void FermerPanneau_Click(object? sender, RoutedEventArgs e)
        => PanneauLateral.IsVisible = false;
}