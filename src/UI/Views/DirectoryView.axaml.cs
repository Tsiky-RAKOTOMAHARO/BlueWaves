using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using UI.ViewModels;
using Core.Models;

namespace UI.Views;

public partial class DirectoryView : UserControl
{
    private ClientViewModel?      _clientVM;
    private FournisseurViewModel? _fournisseurVM;

    public DirectoryView()
    {
        InitializeComponent();

        if (Program.ServiceHost != null)
        {
            _clientVM      = Program.ServiceHost.Services.GetRequiredService<ClientViewModel>();
            _fournisseurVM = Program.ServiceHost.Services.GetRequiredService<FournisseurViewModel>();
            DataContext    = new { ClientVM = _clientVM, FournisseurVM = _fournisseurVM };
            this.AttachedToVisualTree += OnAttachedToVisualTree;
        }
    }

    private async void OnAttachedToVisualTree(object? sender, EventArgs e)
    {
        if (_clientVM == null || _fournisseurVM == null) return;
        try
        {
            if (_clientVM.Clients.Count == 0)
                await _clientVM.LoadClients();
            if (_fournisseurVM.Fournisseurs.Count == 0)
                await _fournisseurVM.LoadFournisseur();
        }
        catch (Exception ex) { Debug.WriteLine($"Load error: {ex.Message}"); }
    }

    // Panneau Client

    private void OpenClientPanel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _clientVM?.ResetForm();
        ClientFormPanel.IsVisible = true;
    }

    private void CloseClientPanel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => ClientFormPanel.IsVisible = false;

    private async void SaveContact_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_clientVM == null) return;
        await _clientVM.SaveClient();
        if (string.IsNullOrEmpty(_clientVM.ErrorMessage))
            ClientFormPanel.IsVisible = false;
    }
    private void EditClient_Click(object? sender, RoutedEventArgs e)
    {
    if (sender is MenuItem mi && mi.Tag is Client client && _clientVM != null)
    {
        _clientVM.LoadClientForEdit(client);
        ClientFormPanel.IsVisible = true;
    }
    }
    private async void DeleteClient_Click(object? sender, RoutedEventArgs e)
    {
    if (sender is MenuItem mi && mi.Tag is Client client && _clientVM != null)
    {
        await _clientVM.RemoveClient(client);
    }
    }
    // Panneau Fournisseur

    private void OpenFournisseurPanel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _fournisseurVM?.ResetForm();
        FournisseurFormPanel.IsVisible = true;
    }

    private void CloseFournisseurPanel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => FournisseurFormPanel.IsVisible = false;

    private async void SaveFournisseur_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_fournisseurVM == null) return;
        await _fournisseurVM.SaveFournisseur();
        if (string.IsNullOrEmpty(_fournisseurVM.ErrorMessage))
            FournisseurFormPanel.IsVisible = false;
    }
     private void EditFournisseur_Click(object? sender, RoutedEventArgs e)
    {
    if (sender is MenuItem mi && mi.Tag is Fournisseur fournisseur && _fournisseurVM != null)
    {
        _fournisseurVM.LoadFournisseurForEdit(fournisseur);
        FournisseurFormPanel.IsVisible = true;
    }
    }
    private async void DeleteFournisseur_Click(object? sender, RoutedEventArgs e)
    {
    if (sender is MenuItem mi && mi.Tag is Fournisseur fournisseur && _fournisseurVM != null)
    {
        await _fournisseurVM.RemoveFournisseur(fournisseur);
    }
    }

    private void OnClientSearchChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        // if (sender is not TextBox searchBox || _clientVM == null) return;
        // _clientVM.SearchQuery = searchBox.Text ?? string.Empty;
    }

    private void OnFournisseurSearchChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        // if (sender is not TextBox searchBox || _fournisseurVM == null) return;
        // _fournisseurVM.SearchQuery = searchBox.Text ?? string.Empty;
    }
}