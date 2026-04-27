using System.Collections.ObjectModel;
using System.Linq;
using Core.Interfaces;
using Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class ApprovisionnementViewModel : ViewModelBase
{
    private readonly IApprovisionnementRepository _repository;

    public ObservableCollection<Approvisionnement> Approvisionnements { get; } = new();
    public ObservableCollection<Approvisionnement> FilteredApprovisionnements { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _searchQuery;

    public ApprovisionnementViewModel(IApprovisionnementRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        IsLoading = true;

        Approvisionnements.Clear();
        FilteredApprovisionnements.Clear();

        var data = await _repository.GetAllApprovisionnement();

        foreach (var item in data)
        {
            Approvisionnements.Add(item);
            FilteredApprovisionnements.Add(item);
        }

        IsLoading = false;
    }

    partial void OnSearchQueryChanged(string? value)
    {
        FilterAppro(value ?? "");
    }


    // Features
    public void FilterAppro(string query)
    {
        FilteredApprovisionnements.Clear();

        var results = string.IsNullOrWhiteSpace(query)
            ? Approvisionnements
            : Approvisionnements.Where(a =>
                a.Certificat != null && a.Certificat.Contains(query, StringComparison.OrdinalIgnoreCase)
            );

        foreach (var item in results)
        {
            FilteredApprovisionnements.Add(item);
        }
    }

    public async Task AddApprovisionnementAsync(Approvisionnement approvisionnement)
    {
        await _repository.AddApprovisionnement(approvisionnement);
        await LoadDataAsync();
    }
}