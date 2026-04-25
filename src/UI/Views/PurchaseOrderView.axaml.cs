using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace UI.Views;

public partial class PurchaseOrderView : UserControl
{
    public PurchaseOrderView()
    {
        InitializeComponent();
    }

    // ─── Appro ────────────────────────────────────────────────
    private void OnNouvelleReceptionClick(object? sender, RoutedEventArgs e)
    {
        // Ouvrir la fenêtre / dialog de création d'une réception
        // var dialog = new NouvelleReceptionWindow();
        // await dialog.ShowDialog(TopLevel.GetTopLevel(this) as Window);
    }

    private void OnApproCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border card && card.Tag is string reference)
        {
            // Ouvrir le détail de la réception
            // var detail = new ReceptionDetailWindow(reference);
            // await detail.ShowDialog(...);
        }
    }

    private void OnApproActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string reference)
        {
            // Afficher un menu contextuel : Éditer / Valider / Supprimer
        }
    }

    private void OnApproSearchChanged(object? sender, TextChangedEventArgs e)
    {
        var query = SearchAppro.Text?.ToLower() ?? "";
        // Filtrer ApproList selon la query
    }

    private void OnApproFiltrerClick(object? sender, RoutedEventArgs e) { }
    private void OnApproExporterClick(object? sender, RoutedEventArgs e) { }

    // ─── Export ───────────────────────────────────────────────
    private void OnNouvelleCommandeClick(object? sender, RoutedEventArgs e)
    {
        // var dialog = new NouvelleCommandeExportWindow();
        // await dialog.ShowDialog(...);
    }

    private void OnExportCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border card && card.Tag is string reference)
        {
            // Ouvrir le détail de la commande export
        }
    }

    private void OnExportActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string reference)
        {
            // Menu contextuel export
        }
    }

    private void OnExportSearchChanged(object? sender, TextChangedEventArgs e)
    {
        var query = SearchExport.Text?.ToLower() ?? "";
        // Filtrer ExportList selon la query
    }

    private void OnExportFiltrerClick(object? sender, RoutedEventArgs e) { }
    private void OnExportExporterClick(object? sender, RoutedEventArgs e) { }
}
