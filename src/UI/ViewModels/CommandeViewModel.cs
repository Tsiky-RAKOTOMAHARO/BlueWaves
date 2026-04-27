using System.Collections.ObjectModel;
using System.Linq;
using Core.Interfaces;
using Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;


namespace UI.ViewModels;

public partial class CommandeViewModel : ViewModelBase
{
    private readonly ICommandeRepository _repository;

    public ObservableCollection<Commande> Commandes { get; } = new();
    public ObservableCollection<Commande> FilteredCommandes { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _searchQuery;

    public CommandeViewModel(ICommandeRepository repository)
    {
        _repository = repository;
        
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        IsLoading = true;

        Commandes.Clear();
        FilteredCommandes.Clear();

        var data = await _repository.GetAllCommande();

        foreach (var item in data)
        {
            Commandes.Add(item);
            FilteredCommandes.Add(item);
        }

        IsLoading = false;
    }

    partial void OnSearchQueryChanged(string? value)
    {
        FilterCommandes(value ?? "");
    }

    public void FilterCommandes(string query)
    {
        FilteredCommandes.Clear();

        var results = string.IsNullOrWhiteSpace(query)
            ? Commandes
            : Commandes.Where(c =>
                (c.Destination != null && c.Destination.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                c.NumeroCommande.ToString().Contains(query)
            );

        foreach (var item in results)
        {
            FilteredCommandes.Add(item);
        }
    }

    public async Task AddCommandeAsync(Commande commande)
    {
        await _repository.AddCommande(commande);
        await LoadDataAsync();
    }
}