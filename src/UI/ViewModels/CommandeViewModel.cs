using System.Collections.ObjectModel;
using System.Linq;
using Core.Models;
using Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace UI.ViewModels;

public partial class CommandeViewModel : ViewModelBase
{
    private readonly CommandeServices _commandeService;

    public ObservableCollection<Commande> Commandes         { get; } = new();
    public ObservableCollection<Commande> FilteredCommandes { get; } = new();

    [ObservableProperty] private bool     _isLoading;
    [ObservableProperty] private string   _destination  = string.Empty;
    [ObservableProperty] private DateTime _dateCommande = DateTime.Now;
    [ObservableProperty] private int      _refClient;
    [ObservableProperty] private int      _codeExport;
    [ObservableProperty] private string   _errorMessage = string.Empty;
    [ObservableProperty] private string?  _searchQuery;

    partial void OnSearchQueryChanged(string? value)
        => FilterCommandes(value ?? string.Empty);

    public CommandeViewModel(CommandeServices commandeService)
    {
        _commandeService = commandeService;
    }

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
        finally { IsLoading = false; }
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
    public async Task SaveCommande()
    {
        try
        {
            ErrorMessage = string.Empty;
            await _commandeService.AddCommande(Destination, DateCommande, RefClient, CodeExport);
            await LoadCommandes();
            ResetForm();
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task DeleteCommande(Commande commande)
    {
        try
        {
            await _commandeService.DeleteCommande(commande);
            Commandes.Remove(commande);
            FilterCommandes(SearchQuery ?? string.Empty);
        }
        catch (ArgumentNullException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void ResetForm()
    {
        Destination   = string.Empty;
        DateCommande  = DateTime.Now;
        RefClient     = 0;
        CodeExport    = 0;
        ErrorMessage  = string.Empty;
    }
}