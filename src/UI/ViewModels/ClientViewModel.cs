using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;

namespace UI.ViewModels;

public partial class ClientViewModel : ViewModelBase
{
    private readonly ClientServices _clientService;

    public ObservableCollection<Client> Clients         { get; } = new();
    public ObservableCollection<Client> FilteredClients { get; } = new();

    [ObservableProperty] private bool    _isLoading;
    [ObservableProperty] private bool    _isFormVisible;
    [ObservableProperty] private string  _errorMessage = string.Empty;
    [ObservableProperty] private string? _searchQuery;

    [ObservableProperty] private string  _nomClient    = string.Empty;
    [ObservableProperty] private string  _prenomClient = string.Empty;
    [ObservableProperty] private string  _telephone    = string.Empty;
    [ObservableProperty] private Client? _selectedClient;

    public ClientViewModel(ClientServices clientService)
    {
        _clientService = clientService;
    }

    [RelayCommand]
    private void NouveauClient()
    {
        ResetForm();
        IsFormVisible = true;
    }

    [RelayCommand]
    private void FermerFormulaire()
    {
        ResetForm();
        IsFormVisible = false;
    }

    [RelayCommand]
    private async Task Sauvegarder()
    {
        if (string.IsNullOrWhiteSpace(NomClient))
        {
            ErrorMessage = "Le nom du client est obligatoire.";
            return;
        }

        try
        {
            ErrorMessage = string.Empty;

            if (SelectedClient is null)
                await _clientService.AddClient(NomClient, PrenomClient, Telephone);
            else
            {
                SelectedClient.NomClient    = NomClient;
                SelectedClient.PrenomClient = PrenomClient;
                SelectedClient.Telephone    = Telephone;
                await _clientService.UpdateClient(SelectedClient);
            }

            await LoadDataAsync();
            ResetForm();
            IsFormVisible = false;
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private async Task Supprimer(Client? client)
    {
        if (client is null) return;
        try
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            await _clientService.DeleteClient(client);
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.InnerException?.Message.Contains("foreign key constraint fails") == true ||
                           ex.Message.Contains("foreign key")
                ? "Impossible de supprimer ce client car il possède un historique de commandes."
                : "Erreur lors de la suppression : " + ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void LoadClientForEdit(Client client)
    {
        SelectedClient = client;
        NomClient      = client.NomClient;
        PrenomClient   = client.PrenomClient;
        Telephone      = client.Telephone;
        ErrorMessage   = string.Empty;
        IsFormVisible  = true;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            var data = await _clientService.GetAllClient();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Clients.Clear();
                FilteredClients.Clear();
                foreach (var c in data)
                {
                    Clients.Add(c);
                    FilteredClients.Add(c);
                }
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erreur de chargement.";
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ResetForm()
    {
        SelectedClient = null;
        NomClient      = string.Empty;
        PrenomClient   = string.Empty;
        Telephone      = string.Empty;
        ErrorMessage   = string.Empty;
    }

    partial void OnSearchQueryChanged(string? value)
    {
        var query   = value ?? string.Empty;
        var results = string.IsNullOrWhiteSpace(query)
            ? Clients
            : Clients.Where(c =>
                (c.NomClient?.Contains(query,    StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.PrenomClient?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Telephone?.Contains(query,    StringComparison.OrdinalIgnoreCase) ?? false));

        FilteredClients.Clear();
        foreach (var item in results)
            FilteredClients.Add(item);
    }
}