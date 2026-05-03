using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace UI.ViewModels;

public partial class CommandeViewModel : ViewModelBase
{
    private readonly CommandeServices _commandeService;
    private readonly AchatServices    _achatService;

    public ObservableCollection<Commande>   Commandes        { get; } = new();
    public ObservableCollection<Commande>   FilteredCommandes { get; } = new();
    public ObservableCollection<AchatLigne> Lignes           { get; } = new();
    

    [ObservableProperty] private bool     _isLoading;
    [ObservableProperty] private string   _destination  = string.Empty;
    [ObservableProperty] private DateTime _dateCommande = DateTime.Now;
    [ObservableProperty] private int      _delai;
    [ObservableProperty] private int      _refClient;

    [ObservableProperty] private int _codeProduit;
    [ObservableProperty] private int _numeroStock;
    [ObservableProperty] private int _quantite;

    [ObservableProperty] private string?   _searchQuery;
    [ObservableProperty] private string    _errorMessage = string.Empty;
    [ObservableProperty] private Commande? _selectedCommande;

    public CommandeViewModel(CommandeServices commandeService, AchatServices achatService)
    {
        _commandeService = commandeService;
        _achatService    = achatService;
    }

    partial void OnSearchQueryChanged(string? value)
        => FilterCommandes(value ?? string.Empty);

    [RelayCommand]
    public async Task LoadCommandes()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Commandes.Clear();
            FilteredCommandes.Clear();

            var data = await _commandeService.GetAllCommande();

            foreach (var item in data)
            {
                Commandes.Add(item);
                FilteredCommandes.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void FilterCommandes(string query)
    {
        FilteredCommandes.Clear();

        var results = string.IsNullOrWhiteSpace(query)
            ? Commandes
            : Commandes.Where(c =>
                (c.Destination != null && c.Destination.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                c.NumeroCommande.ToString().Contains(query));

        foreach (var item in results)
            FilteredCommandes.Add(item);
    }

    [RelayCommand]
    public void AjouterLigne()
    {
        if (CodeProduit <= 0 || NumeroStock <= 0 || Quantite <= 0)
        {
            ErrorMessage = "Produit, stock et quantité obligatoires.";
            return;
        }

        Lignes.Add(new AchatLigne
        {
            CodeProduit = CodeProduit,
            NumeroStock = NumeroStock,
            Quantite    = Quantite
        });

        CodeProduit = 0;
        NumeroStock = 0;
        Quantite    = 0;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    public void RetirerLigne(AchatLigne ligne)
        => Lignes.Remove(ligne);

    [RelayCommand]
    public async Task ConfirmerCommande()
    {
        if (!Lignes.Any())
        {
            ErrorMessage = "Ajoutez au moins une ligne.";
            return;
        }

        try
        {
            ErrorMessage = string.Empty;

            var commande = await _commandeService.AddCommande(
                Destination, DateCommande, Delai, RefClient);

            foreach (var ligne in Lignes)
            {
                await _achatService.AddAchat(new Achat
                {
                    NumeroCommande = commande.NumeroCommande,
                    CodeProduit    = ligne.CodeProduit,
                    NumeroStock    = ligne.NumeroStock,
                    Quantite       = ligne.Quantite
                });
            }

            await LoadCommandes();
            ResetForm();
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    public async Task DeleteCommande(Commande commande)
    {
        try
        {
            await _commandeService.DeleteCommande(commande);
            Commandes.Remove(commande);
            FilterCommandes(SearchQuery ?? string.Empty);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void ResetForm()
    {
        Destination  = string.Empty;
        DateCommande = DateTime.Now;
        Delai        = 0;
        RefClient    = 0;
        Lignes.Clear();
        ErrorMessage = string.Empty;
    }
}