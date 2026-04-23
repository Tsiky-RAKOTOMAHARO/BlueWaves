using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Core.Interfaces;
using Core.Models;

namespace UI.ViewModels;

public partial class ClientViewModel : ViewModelBase
{
    private readonly IClientRepository _repository;

    public ObservableCollection<Client> Clients { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _refClient;
    [ObservableProperty] private string? _nomClient;
    [ObservableProperty] private string? _prenomClient;
    [ObservableProperty] private string? _telephone;

    public ClientViewModel(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Clients.Clear();

            var data = await _repository.GetAllClient();
            
            foreach (var item in data)
            {
                Clients.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}