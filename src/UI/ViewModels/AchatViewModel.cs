using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Core.Models;
using Core.Services;

namespace UI.ViewModels;

public partial class AchatViewModel : ViewModelBase
{
    private readonly AchatServices _achatService;

    public ObservableCollection<Achat> Achats { get; } = new();
    public ObservableCollection<Achat> FilteredAchats { get; } = new();

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private int _idAchat;
    [ObservableProperty] private int _codeProduit;
    [ObservableProperty] private int _numeroCommande;
    [ObservableProperty] private int _quantite;

    [ObservableProperty] private string _searchQuery = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public AchatViewModel(AchatServices achatService)
    {
        _achatService = achatService;
    }

    partial void OnSearchQueryChanged(string value)
        => FilterAchats(value);

    [RelayCommand]
    public async Task LoadAchats()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Achats.Clear();
            FilteredAchats.Clear();

            var data = await _achatService.GetAllAchat();

            foreach (var item in data)
            {
                Achats.Add(item);
                FilteredAchats.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void FilterAchats(string query)
    {
        FilteredAchats.Clear();

        var result = string.IsNullOrWhiteSpace(query)
            ? Achats
            : Achats.Where(a =>
                a.IdAchat.ToString().Contains(query) ||
                a.NumeroCommande.ToString().Contains(query) ||
                a.CodeProduit.ToString().Contains(query));

        foreach (var item in result)
            FilteredAchats.Add(item);
    }

    [RelayCommand]
    public async Task SaveAchat()
    {
        try
        {
            ErrorMessage = string.Empty;

            await _achatService.AddAchat(
                CodeProduit,
                NumeroCommande,
                Quantite
            );

            await LoadAchats();
            ResetForm();
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    public async Task DeleteAchat(Achat achat)
    {
        try
        {
            await _achatService.DeleteAchat(achat);

            Achats.Remove(achat);
            FilterAchats(SearchQuery);
        }
        catch (ArgumentNullException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void ResetForm()
    {
        IdAchat = 0;
        CodeProduit = 0;
        NumeroCommande = 0;
        Quantite = 0;
        ErrorMessage = string.Empty;
    }
}