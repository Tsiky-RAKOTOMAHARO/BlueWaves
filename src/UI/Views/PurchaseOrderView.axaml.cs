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
    private PurchaseOrderViewModel? _viewModel;
    private FournisseurViewModel? _fournisseurViewModel;
    private ProduitViewModel? _produitViewModel;
    private ClientViewModel? _clientViewModel;
    private ExportViewModel? _exportViewModel;
    private bool _isInitialized;

    public PurchaseOrderView()
    {
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            var approVM = Program.ServiceHost.Services.GetRequiredService<ApprovisionnementViewModel>();
            var commandeVM = Program.ServiceHost.Services.GetRequiredService<CommandeViewModel>();
            _fournisseurViewModel = Program.ServiceHost.Services.GetRequiredService<FournisseurViewModel>();
            _produitViewModel = Program.ServiceHost.Services.GetRequiredService<ProduitViewModel>();
            _clientViewModel = Program.ServiceHost.Services.GetRequiredService<ClientViewModel>();
            _exportViewModel = Program.ServiceHost.Services.GetRequiredService<ExportViewModel>();
            _viewModel = new PurchaseOrderViewModel(approVM, commandeVM);
            DataContext = _viewModel;

            AttachedToVisualTree += OnAttachedToVisualTree;
        }
    }

    private async void OnAttachedToVisualTree(object? sender, EventArgs e)
    {
        if (_viewModel == null || _isInitialized)
        {
            return;
        }

        try
        {
            await _viewModel.InitializeAsync();
            await LoadReferenceDataAsync();
            PopulateApproDropdowns();
            PopulateCommandeDropdowns();
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            // Avoid crashing the UI thread when repositories/DB throw.
            Debug.WriteLine($"PurchaseOrderView initialization error: {ex.Message}");
        }
    }

    private async void OnNouvelleReceptionClick(object? sender, RoutedEventArgs e)
    {
        await EnsureApproReferenceDataAsync();
        PopulateApproDropdowns();

        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
        {
            panel.IsVisible = true;
        }
    }

    private void OnApproFormClose(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
        {
            panel.IsVisible = false;
        }
    }

    private void OnApproSearchChanged(object? sender, TextChangedEventArgs e)
    {
        if (_viewModel == null || sender is not TextBox searchBox)
        {
            return;
        }

        _viewModel.ApprovisionnementVM.FilterAppro(searchBox.Text ?? string.Empty);
    }

    private void OnApproCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
        {
            panel.IsVisible = true;
        }
    }

    private void OnApproActionsClick(object? sender, RoutedEventArgs e)
    {
        // Hook kept for future context menu/actions implementation.
    }

    private async void OnApproSave(object? sender, RoutedEventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }

        var certificat = this.FindControl<TextBox>("ApproCertificat")?.Text?.Trim() ?? string.Empty;
        var selectedFournisseur = this.FindControl<ComboBox>("ApproRefFournisseurCombo")?.SelectedItem as Fournisseur;
        var selectedProduit = this.FindControl<ComboBox>("ApproCodeProduitCombo")?.SelectedItem as Produit;

        if (this.FindControl<TextBlock>("ApproErrorText") is { } approError)
        {
            approError.IsVisible = false;
            approError.Text = string.Empty;
        }

        if (string.IsNullOrWhiteSpace(certificat) ||
            selectedFournisseur == null ||
            selectedProduit == null)
        {
            ShowError("ApproErrorText", "Veuillez renseigner certificat, fournisseur et produit.");
            return;
        }

        var item = new Approvisionnement
        {
            Certificat = certificat,
            RefFournisseur = selectedFournisseur.RefFournisseur,
            CodeProduit = selectedProduit.CodeProduit
        };

        try
        {
            await _viewModel.ApprovisionnementVM.AddApprovisionnementAsync(item);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"PurchaseOrderView save refresh error: {ex.Message}");
            ShowError("ApproErrorText", "Impossible d'enregistrer la réception.");
            return;
        }

        ClearTextBox("ApproCertificat");
        ResetComboBox("ApproRefFournisseurCombo");
        ResetComboBox("ApproCodeProduitCombo");

        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
        {
            panel.IsVisible = false;
        }
    }

    private async void OnNouvelleCommandeClick(object? sender, RoutedEventArgs e)
    {
        await EnsureCommandeReferenceDataAsync();
        PopulateCommandeDropdowns();

        if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
        {
            panel.IsVisible = true;
        }
    }

    private void OnCommandeFormClose(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
        {
            panel.IsVisible = false;
        }
    }

    private void OnCommandeSearchChanged(object? sender, TextChangedEventArgs e)
    {
        if (_viewModel == null || sender is not TextBox searchBox)
        {
            return;
        }

        _viewModel.CommandeVM.FilterCommandes(searchBox.Text ?? string.Empty);
    }

    private void OnCommandeCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
        {
            panel.IsVisible = true;
        }
    }

    private void OnCommandeActionsClick(object? sender, RoutedEventArgs e)
    {
        // Hook kept for future context menu/actions implementation.
    }

    private async void OnCommandeSave(object? sender, RoutedEventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }

        if (this.FindControl<TextBlock>("CommandeErrorText") is { } cmdError)
        {
            cmdError.IsVisible = false;
            cmdError.Text = string.Empty;
        }

        var destination = this.FindControl<TextBox>("CommandeDestination")?.Text?.Trim() ?? string.Empty;
        var selectedClient = this.FindControl<ComboBox>("CommandeRefClientCombo")?.SelectedItem as Client;
        var date = this.FindControl<DatePicker>("CommandeDate")?.SelectedDate?.Date;

        if (string.IsNullOrWhiteSpace(destination) ||
            selectedClient == null ||
            date == null)
        {
            ShowError("CommandeErrorText", "Veuillez renseigner destination, date et client.");
            return;
        }

        if (_exportViewModel == null)
        {
            ShowError("CommandeErrorText", "Service export indisponible.");
            return;
        }

        Export newExport;
        try
        {
            newExport = await _exportViewModel.AddExportAsync(new Export { Delai = 0 });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"PurchaseOrderView create export error: {ex.Message}");
            ShowError("CommandeErrorText", "Impossible de générer le numéro export.");
            return;
        }

        var commande = new Commande
        {
            Destination = destination,
            RefClient = selectedClient.RefClient,
            CodeExport = newExport.NumeroExport,
            DateCommande = date.Value
        };

        try
        {
            await _viewModel.CommandeVM.AddCommandeAsync(commande);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"PurchaseOrderView save commande error: {ex.Message}");
            ShowError("CommandeErrorText", "Impossible d'enregistrer la commande.");
            return;
        }

        ClearTextBox("CommandeDestination");
        ResetComboBox("CommandeRefClientCombo");
        if (this.FindControl<DatePicker>("CommandeDate") is { } cmdDate)
        {
            cmdDate.SelectedDate = null;
        }

        if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
        {
            panel.IsVisible = false;
        }
    }

    private void ClearTextBox(string controlName)
    {
        if (this.FindControl<TextBox>(controlName) is { } textBox)
        {
            textBox.Text = string.Empty;
        }
    }

    private void ResetComboBox(string controlName)
    {
        if (this.FindControl<ComboBox>(controlName) is { } comboBox)
        {
            comboBox.SelectedItem = null;
        }
    }

    private void ShowError(string textBlockName, string message)
    {
        if (this.FindControl<TextBlock>(textBlockName) is { } errorText)
        {
            errorText.Text = message;
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
            {
                await _fournisseurViewModel.LoadFournisseur();
            }

            if (_produitViewModel != null && _produitViewModel.Produits.Count == 0)
            {
                await _produitViewModel.LoadProduits();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to load appro reference data: {ex.Message}");
        }
    }

    private async Task EnsureCommandeReferenceDataAsync()
    {
        try
        {
            if (_clientViewModel != null && _clientViewModel.Clients.Count == 0)
            {
                await _clientViewModel.LoadClients();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to load commande reference data: {ex.Message}");
        }
    }

    private void PopulateApproDropdowns()
    {
        if (this.FindControl<ComboBox>("ApproRefFournisseurCombo") is { } approFournisseurCombo)
        {
            approFournisseurCombo.ItemsSource = _fournisseurViewModel?.Fournisseurs;
        }

        if (this.FindControl<ComboBox>("ApproCodeProduitCombo") is { } approProduitCombo)
        {
            approProduitCombo.ItemsSource = _produitViewModel?.Produits;
        }
    }

    private void PopulateCommandeDropdowns()
    {
        if (this.FindControl<ComboBox>("CommandeRefClientCombo") is { } cmdClientCombo)
        {
            cmdClientCombo.ItemsSource = _clientViewModel?.Clients;
        }
    }
}