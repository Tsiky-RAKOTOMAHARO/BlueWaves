using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Models;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using UI.ViewModels;

namespace UI.Views;

public partial class PurchaseOrderView : UserControl
{
    private ApprovisionnementViewModel? _approVM;
    private CommandeViewModel? _commandeVM;
    private ClientViewModel? _clientViewModel;
    private ExportViewModel? _exportViewModel;

    private bool _isInitialized;

    public PurchaseOrderView()
    {
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            _approVM = Program.ServiceHost.Services.GetRequiredService<ApprovisionnementViewModel>();
            _commandeVM = Program.ServiceHost.Services.GetRequiredService<CommandeViewModel>();
            _clientViewModel = Program.ServiceHost.Services.GetRequiredService<ClientViewModel>();
            _exportViewModel = Program.ServiceHost.Services.GetRequiredService<ExportViewModel>();

            DataContext = new { ApproVM = _approVM, CommandeVM = _commandeVM };
            // AttachedToVisualTree += OnAttachedToVisualTree;
        }
    }

    protected override async void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
{
    base.OnAttachedToVisualTree(e);

    if (!_isInitialized)
    {
        try
        {
            if (_commandeVM != null && _commandeVM.Commandes.Count == 0)
                await _commandeVM.LoadCommandesCommand.ExecuteAsync(null);

            if (_clientViewModel != null && _clientViewModel.Clients.Count == 0)
                await _clientViewModel.LoadClients();

            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Init error: {ex.Message}");
        }
    }

    try
    {
        if (_approVM != null)
        {
            await _approVM.LoadDataAsync();
            PopulateApproDropdowns();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Appro reload error: {ex.Message}");
    }
}
   

    //Approvisionnement

    private void OnNouvelleReceptionClick(object? sender, RoutedEventArgs e)
    {
        _approVM?.ResetForm();

        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
            panel.IsVisible = true;
    }

    private void OnApproFormClose(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
            panel.IsVisible = false;
    }

    private void OnApproSearchChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox searchBox || _approVM == null) return;
        _approVM.SearchQuery = searchBox.Text ?? string.Empty;
    }

    private void OnApproCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is Approvisionnement appro)
            Debug.WriteLine($"Appro sélectionné : {appro.Certificat}");
    }

    private void OnApproActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Approvisionnement appro)
            Debug.WriteLine($"Actions pour : {appro.Certificat}");
    }

    private async void OnApproSave(object? sender, RoutedEventArgs e)
    {
        if (_approVM == null) return;

        var certificat = this.FindControl<TextBox>("ApproCertificat")?.Text?.Trim();
        var quantiteText = this.FindControl<TextBox>("ApproQuantite")?.Text?.Trim();
        var selectedFournisseur = this.FindControl<ComboBox>("ApproRefFournisseurCombo")?.SelectedItem as Fournisseur;
        var selectedProduit = this.FindControl<ComboBox>("ApproCodeProduitCombo")?.SelectedItem as Produit;
        var selectedStock = this.FindControl<ComboBox>("ApproStockCombo")?.SelectedItem as Stock;

        if (string.IsNullOrWhiteSpace(certificat) ||
            selectedFournisseur == null ||
            selectedProduit == null ||
            selectedStock == null ||
            !int.TryParse(quantiteText, out int quantite) ||
            quantite <= 0)
        {
            ShowError("ApproErrorText", "Veuillez remplir correctement tous les champs.");
            return;
        }

        try
        {
            _approVM.CertificatSaisi = certificat;
            _approVM.SelectedFournisseur = selectedFournisseur;
            _approVM.SelectedProduit = selectedProduit;
            _approVM.SelectedStock = selectedStock;
            _approVM.QuantiteSaisie = quantite;

            await _approVM.AddApprovisionnementAsync();

            if (!string.IsNullOrEmpty(_approVM.ErrorMessage))
            {
                ShowError("ApproErrorText", _approVM.ErrorMessage);
                return;
            }

            ClearTextBox("ApproCertificat");
            ClearTextBox("ApproQuantite");
            ResetComboBox("ApproRefFournisseurCombo");
            ResetComboBox("ApproCodeProduitCombo");
            ResetComboBox("ApproStockCombo");

            if (this.FindControl<Border>("ApproFormPanel") is { } panel)
                panel.IsVisible = false;
        }
        catch (Exception ex)
        {
            ShowError("ApproErrorText", ex.Message);
        }
    }

    private async void OnDeleteApproClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Approvisionnement appro && _approVM != null)
            await _approVM.DeleteApprovisionnementAsync(appro);
    }

    // ─── Commandes Export 

    private async void OnNouvelleCommandeClick(object? sender, RoutedEventArgs e)
    {
        if (_clientViewModel != null && _clientViewModel.Clients.Count == 0)
            await _clientViewModel.LoadClients();

        PopulateCommandeDropdowns();
        _commandeVM?.ResetForm();

        if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
            panel.IsVisible = true;
    }

    private void OnCommandeFormClose(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
            panel.IsVisible = false;
    }

    private void OnCommandeSearchChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox searchBox) return;
        _commandeVM?.FilterCommandes(searchBox.Text ?? string.Empty);
    }

    private void OnCommandeCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is Commande commande)
            Debug.WriteLine($"Commande sélectionnée : {commande.NumeroCommande}");
    }

    private void OnCommandeActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Commande commande)
            Debug.WriteLine($"Actions pour : {commande.NumeroCommande}");
    }

    private async void OnCommandeSave(object? sender, RoutedEventArgs e)
    {
        if (_commandeVM == null) return;

        var selectedClient = this.FindControl<ComboBox>("CommandeRefClientCombo")?.SelectedItem as Client;
        var date = this.FindControl<DatePicker>("CommandeDate")?.SelectedDate?.Date;

        if (string.IsNullOrWhiteSpace(_commandeVM.Destination) ||
            selectedClient == null ||
            date == null)
        {
            ShowError("CommandeErrorText", "Champs obligatoires manquants.");
            return;
        }

        try
        {
            if (_exportViewModel == null)
                throw new Exception("ExportViewModel non initialisé");

            var newExport = await _exportViewModel.AddExportAsync(new Export { Delai = 0 });

            _commandeVM.RefClient = selectedClient.RefClient;
            _commandeVM.DateCommande = date.Value;
            _commandeVM.CodeExport = newExport.NumeroExport;

            await _commandeVM.SaveCommandeCommand.ExecuteAsync(null);

            if (string.IsNullOrEmpty(_commandeVM.ErrorMessage))
            {
                if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
                    panel.IsVisible = false;
            }
            else
            {
                ShowError("CommandeErrorText", _commandeVM.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            ShowError("CommandeErrorText", ex.Message);
        }
    }

    // ─── Helpers ─────────────────────────────────────────────────────────────

    private void ShowError(string name, string message)
    {
        if (this.FindControl<TextBlock>(name) is { } tb)
        {
            tb.Text = message;
            tb.IsVisible = true;
        }
    }

    private void ClearTextBox(string name)
    {
        if (this.FindControl<TextBox>(name) is { } tb)
            tb.Text = string.Empty;
    }

    private void ResetComboBox(string name)
    {
        if (this.FindControl<ComboBox>(name) is { } cb)
            cb.SelectedItem = null;
    }

    private void PopulateApproDropdowns()
    {
        if (this.FindControl<ComboBox>("ApproRefFournisseurCombo") is { } f)
            f.ItemsSource = _approVM?.Fournisseurs;

        if (this.FindControl<ComboBox>("ApproCodeProduitCombo") is { } p)
            p.ItemsSource = _approVM?.Produits;

        if (this.FindControl<ComboBox>("ApproStockCombo") is { } s)
            s.ItemsSource = _approVM?.Stocks;
    }

    private void PopulateCommandeDropdowns()
    {
        if (this.FindControl<ComboBox>("CommandeRefClientCombo") is { } c)
            c.ItemsSource = _clientViewModel?.Clients;
    }
}
