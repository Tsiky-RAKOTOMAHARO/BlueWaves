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
    private CommandeViewModel?          _commandeVM;
    private FournisseurViewModel?       _fournisseurViewModel;
    private ProduitViewModel?           _produitViewModel;
    private ClientViewModel?            _clientViewModel;
    private ExportViewModel?            _exportViewModel;
    private bool                        _isInitialized;

    public PurchaseOrderView()
    {
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            _approVM              = Program.ServiceHost.Services.GetRequiredService<ApprovisionnementViewModel>();
            _commandeVM           = Program.ServiceHost.Services.GetRequiredService<CommandeViewModel>();
            _fournisseurViewModel = Program.ServiceHost.Services.GetRequiredService<FournisseurViewModel>();
            _produitViewModel     = Program.ServiceHost.Services.GetRequiredService<ProduitViewModel>();
            _clientViewModel      = Program.ServiceHost.Services.GetRequiredService<ClientViewModel>();
            _exportViewModel      = Program.ServiceHost.Services.GetRequiredService<ExportViewModel>();
            DataContext            = new { ApproVM = _approVM, CommandeVM = _commandeVM };
            AttachedToVisualTree  += OnAttachedToVisualTree;
        }
    }

    private async void OnAttachedToVisualTree(object? sender, EventArgs e)
{
    if (_isInitialized) return;
    try
    {
        await LoadReferenceDataAsync();
        if (_approVM?.Approvisionnements.Count == 0)
            await _approVM.LoadDataAsync();
        if (_commandeVM?.Commandes.Count == 0)
            await _commandeVM.LoadCommandesCommand.ExecuteAsync(null);
        PopulateApproDropdowns();
        PopulateCommandeDropdowns();
        _isInitialized = true;
    }
    catch (Exception ex) { Debug.WriteLine($"PurchaseOrderView initialization error: {ex.Message}"); }
}

    // ── Appro 

    private async void OnNouvelleReceptionClick(object? sender, RoutedEventArgs e)
    {
        await EnsureApproReferenceDataAsync();
        PopulateApproDropdowns();
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
        if (sender is not TextBox searchBox) return;
        _approVM?.FilterAppro(searchBox.Text ?? string.Empty);
    }

    private void OnApproCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
            panel.IsVisible = true;
    }

    private void OnApproActionsClick(object? sender, RoutedEventArgs e) { }

    private async void OnApproSave(object? sender, RoutedEventArgs e)
    {
        if (_approVM == null) return;

        var certificat          = this.FindControl<TextBox>("ApproCertificat")?.Text?.Trim() ?? string.Empty;
        var selectedFournisseur = this.FindControl<ComboBox>("ApproRefFournisseurCombo")?.SelectedItem as Fournisseur;
        var selectedProduit     = this.FindControl<ComboBox>("ApproCodeProduitCombo")?.SelectedItem as Produit;

        if (string.IsNullOrWhiteSpace(certificat) || selectedFournisseur == null || selectedProduit == null)
        {
            ShowError("ApproErrorText", "Veuillez renseigner certificat, fournisseur et produit.");
            return;
        }

        try
        {
            await _approVM.AddApprovisionnementAsync(new Approvisionnement
            {
                Certificat     = certificat,
                RefFournisseur = selectedFournisseur.RefFournisseur,
                CodeProduit    = selectedProduit.CodeProduit
            });

            ClearTextBox("ApproCertificat");
            ResetComboBox("ApproRefFournisseurCombo");
            ResetComboBox("ApproCodeProduitCombo");

            if (this.FindControl<Border>("ApproFormPanel") is { } panel)
                panel.IsVisible = false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Save appro error: {ex.Message}");
            ShowError("ApproErrorText", "Impossible d'enregistrer la réception.");
        }
    }

    // ── Commande 

    private async void OnNouvelleCommandeClick(object? sender, RoutedEventArgs e)
    {
        await EnsureCommandeReferenceDataAsync();
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
        if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
            panel.IsVisible = true;
    }

    private void OnCommandeActionsClick(object? sender, RoutedEventArgs e) { }

    private async void OnCommandeSave(object? sender, RoutedEventArgs e)
    {
        if (_commandeVM == null) return;

        var selectedClient = this.FindControl<ComboBox>("CommandeRefClientCombo")?.SelectedItem as Client;
        var date           = this.FindControl<DatePicker>("CommandeDate")?.SelectedDate?.Date;

        if (string.IsNullOrWhiteSpace(_commandeVM.Destination) || selectedClient == null || date == null)
        {
            ShowError("CommandeErrorText", "Veuillez renseigner destination, date et client.");
            return;
        }

        if (_exportViewModel == null)
        {
            ShowError("CommandeErrorText", "Service export indisponible.");
            return;
        }

        try
        {
            var newExport = await _exportViewModel.AddExportAsync(new Export { Delai = 0 });
            _commandeVM.RefClient    = selectedClient.RefClient;
            _commandeVM.DateCommande = date.Value;
            _commandeVM.CodeExport   = newExport.NumeroExport;

            await _commandeVM.SaveCommandeCommand.ExecuteAsync(null);

            if (string.IsNullOrEmpty(_commandeVM.ErrorMessage))
            {
                ResetComboBox("CommandeRefClientCombo");
                if (this.FindControl<DatePicker>("CommandeDate") is { } cmdDate)
                    cmdDate.SelectedDate = null;
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
            Debug.WriteLine($"Save commande error: {ex.Message}");
            ShowError("CommandeErrorText", "Impossible d'enregistrer la commande.");
        }
    }


    private void ClearTextBox(string controlName)
    {
        if (this.FindControl<TextBox>(controlName) is { } textBox)
            textBox.Text = string.Empty;
    }

    private void ResetComboBox(string controlName)
    {
        if (this.FindControl<ComboBox>(controlName) is { } comboBox)
            comboBox.SelectedItem = null;
    }

    private void ShowError(string textBlockName, string message)
    {
        if (this.FindControl<TextBlock>(textBlockName) is { } errorText)
        {
            errorText.Text      = message;
            errorText.IsVisible = true;
        }
    }

    private async Task LoadReferenceDataAsync()
    {
        await EnsureApproReferenceDataAsync();
        await EnsureCommandeReferenceDataAsync();
    }

    private async Task EnsureApproReferenceDataAsync()
    {
        try
        {
            if (_fournisseurViewModel != null && _fournisseurViewModel.Fournisseurs.Count == 0)
                await _fournisseurViewModel.LoadFournisseur();
            if (_produitViewModel != null && _produitViewModel.Produits.Count == 0)
                await _produitViewModel.LoadProduits();
        }
        catch (Exception ex) { Debug.WriteLine($"Unable to load appro reference data: {ex.Message}"); }
    }

    private async Task EnsureCommandeReferenceDataAsync()
    {
        try
        {
            if (_clientViewModel != null && _clientViewModel.Clients.Count == 0)
                await _clientViewModel.LoadClients();
        }
        catch (Exception ex) { Debug.WriteLine($"Unable to load commande reference data: {ex.Message}"); }
    }

    private void PopulateApproDropdowns()
    {
        if (this.FindControl<ComboBox>("ApproRefFournisseurCombo") is { } approFournisseurCombo)
            approFournisseurCombo.ItemsSource = _fournisseurViewModel?.Fournisseurs;
        if (this.FindControl<ComboBox>("ApproCodeProduitCombo") is { } approProduitCombo)
            approProduitCombo.ItemsSource = _produitViewModel?.Produits;
    }

    private void PopulateCommandeDropdowns()
    {
        if (this.FindControl<ComboBox>("CommandeRefClientCombo") is { } cmdClientCombo)
            cmdClientCombo.ItemsSource = _clientViewModel?.Clients;
    }
}