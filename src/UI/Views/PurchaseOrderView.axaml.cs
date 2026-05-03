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

    private FournisseurViewModel? _fournisseurViewModel;

    private ProduitViewModel? _produitViewModel;
    private StockViewModel? _stockViewModel;

    private bool _isInitialized;

    public PurchaseOrderView()
    {
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            _approVM = Program.ServiceHost.Services.GetRequiredService<ApprovisionnementViewModel>();
            _commandeVM = Program.ServiceHost.Services.GetRequiredService<CommandeViewModel>();
            _clientViewModel = Program.ServiceHost.Services.GetRequiredService<ClientViewModel>();
            _fournisseurViewModel = Program.ServiceHost.Services.GetRequiredService<FournisseurViewModel>();
            _produitViewModel = Program.ServiceHost.Services.GetRequiredService<ProduitViewModel>();
            _stockViewModel = Program.ServiceHost.Services.GetRequiredService<StockViewModel>();

            DataContext = new { 
                ApproVM = _approVM,
                CommandeVM = _commandeVM,
                ClientVM = _clientViewModel,
                FournisseurVM = _fournisseurViewModel,
                ProduitVM = _produitViewModel, 
                StockVM   = _stockViewModel   
            };
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

    public void OnNouvelleReceptionClick(object? sender, RoutedEventArgs e)
    {
        _approVM?.ResetForm();
        ShowError("ApproErrorText", string.Empty);

        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
            panel.IsVisible = true;
    }

    public void OnApproFormClose(object? sender, RoutedEventArgs e)
    {
        ShowError("ApproErrorText", string.Empty);
        if (this.FindControl<Border>("ApproFormPanel") is { } panel)
            panel.IsVisible = false;
    }

    public void OnApproSearchChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox searchBox || _approVM == null) return;
        _approVM.SearchQuery = searchBox.Text ?? string.Empty;
    }

    
    public void OnApproCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is Approvisionnement appro)
            Debug.WriteLine($"Appro sélectionné : {appro.Certificat}");
    }

    public void OnApproActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Approvisionnement appro)
            Debug.WriteLine($"Actions pour : {appro.Certificat}");
    }

    public async void OnApproSave(object? sender, RoutedEventArgs e)
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

    public async void OnDeleteApproClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Approvisionnement appro && _approVM != null)
            await _approVM.DeleteApprovisionnementAsync(appro);
    }

    public void PopulateApproDropdowns()
{
    if (this.FindControl<ComboBox>("ApproRefFournisseurCombo") is { } f)
        f.ItemsSource = _approVM?.Fournisseurs;

    if (this.FindControl<ComboBox>("ApproCodeProduitCombo") is { } p)
        p.ItemsSource = _produitViewModel?.Produits;

    if (this.FindControl<ComboBox>("ApproStockCombo") is { } s)
        s.ItemsSource = _stockViewModel?.Stocks;
}


    // Commande
    public async void OnCommandeSave(object? sender, RoutedEventArgs e)
{
    if (_commandeVM == null) return;

    var selectedClient = this.FindControl<ComboBox>("CommandeRefClientCombo")?.SelectedItem as Client;
    var date = this.FindControl<DatePicker>("CommandeDate")?.SelectedDate?.Date;
    var delaiText = this.FindControl<TextBox>("CommandeDelai")?.Text;

    if (string.IsNullOrWhiteSpace(_commandeVM.Destination) ||
        selectedClient == null ||
        date == null ||
        !int.TryParse(delaiText, out int delai) || delai <= 0)
    {
        ShowError("CommandeErrorText", "Champs obligatoires manquants.");
        return;
    }

    try
    {
        _commandeVM.RefClient    = selectedClient.RefClient;
        _commandeVM.DateCommande = date.Value;
        _commandeVM.Delai        = delai;

        await _commandeVM.ConfirmerCommandeCommand.ExecuteAsync(null);  // ← renommé

        if (string.IsNullOrEmpty(_commandeVM.ErrorMessage))
        {
            if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
                panel.IsVisible = false;

            ClearTextBox("CommandeDestination");
            ClearTextBox("CommandeDelai");
            ResetComboBox("CommandeRefClientCombo");
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

// Ajoute une ligne au panier
private void OnAjouterLigneClick(object? sender, RoutedEventArgs e)
{
    if (_commandeVM == null) return;

    var selectedProduit = this.FindControl<ComboBox>("AchatProduitCombo")?.SelectedItem as Produit;
    var selectedStock   = this.FindControl<ComboBox>("AchatStockCombo")?.SelectedItem as Stock;
    var quantiteText    = this.FindControl<TextBox>("AchatQuantite")?.Text;

    if (selectedProduit == null || selectedStock == null ||
        !int.TryParse(quantiteText, out int quantite) || quantite <= 0)
    {
        ShowError("CommandeErrorText", "Produit, stock et quantité obligatoires.");
        return;
    }

    _commandeVM.CodeProduit = selectedProduit.CodeProduit;
    _commandeVM.NumeroStock = selectedStock.NumeroStock;
    _commandeVM.Quantite    = quantite;

    _commandeVM.AjouterLigneCommand.Execute(null);

    // Reset champs ligne
    ResetComboBox("AchatProduitCombo");
    ResetComboBox("AchatStockCombo");
    ClearTextBox("AchatQuantite");
    ShowError("CommandeErrorText", string.Empty);
}

// Retire une ligne du panier
private void OnRetirerLigneClick(object? sender, RoutedEventArgs e)
{
    if (sender is Button button && button.DataContext is AchatLigne ligne)
        _commandeVM?.RetirerLigneCommand.Execute(ligne);
}

private void OnNouvelleCommandeClick(object? sender, RoutedEventArgs e)
{
    if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
        panel.IsVisible = true;

    PopulateCommandeDropdowns();

    ClearTextBox("CommandeDestination");
    ClearTextBox("CommandeDelai");
    ResetComboBox("CommandeRefClientCombo");
    _commandeVM?.Lignes.Clear();
    ShowError("CommandeErrorText", string.Empty);
}

private void OnCommandeCardClick(object? sender, PointerPressedEventArgs e)
{
    if (sender is Border border && border.DataContext is Commande commande)
    {
        if (_commandeVM == null) return;
        _commandeVM.SelectedCommande = commande;
    }
}

private void OnCommandeActionsClick(object? sender, RoutedEventArgs e)
{
    if (sender is Button button)
    {
        if (button.DataContext is Commande commande && _commandeVM != null)
            _commandeVM.SelectedCommande = commande;

        button.ContextMenu?.Open(button);
    }
}

private async void OnCommandeDeleteClick(object? sender, RoutedEventArgs e)
{
    if (_commandeVM?.SelectedCommande == null) return;

    await _commandeVM.DeleteCommandeCommand.ExecuteAsync(_commandeVM.SelectedCommande);
}

private void OnCommandeAnnulerClick(object? sender, RoutedEventArgs e)
{
    if (this.FindControl<Border>("CommandeFormPanel") is { } panel)
        panel.IsVisible = false;

    ClearTextBox("CommandeDestination");
    ClearTextBox("CommandeDelai");
    ResetComboBox("CommandeRefClientCombo");
    _commandeVM?.Lignes.Clear();
    ShowError("CommandeErrorText", string.Empty);
}
public void PopulateCommandeDropdowns()
{
    if (this.FindControl<ComboBox>("AchatProduitCombo") is { } p)
        p.ItemsSource = _produitViewModel?.Produits;

    if (this.FindControl<ComboBox>("AchatStockCombo") is { } s)
        s.ItemsSource = _stockViewModel?.Stocks;
}
public void ShowError(string name, string message)
{
    if (this.FindControl<TextBlock>(name) is { } tb)
    {
        tb.Text = message;
        tb.IsVisible = !string.IsNullOrWhiteSpace(message);
    }
}
public void ClearTextBox(string name)
{
    if (this.FindControl<TextBox>(name) is { } tb)
        tb.Text = string.Empty;
}
public void ResetComboBox(string name)
{
    if (this.FindControl<ComboBox>(name) is { } cb)
        cb.SelectedItem = null;
}

}
