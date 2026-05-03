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

public partial class FournisseurViewModel : ViewModelBase
{
    private readonly FournisseurServices _fournisseurService;

    public ObservableCollection<Fournisseur> Fournisseurs         { get; } = new();
    public ObservableCollection<Fournisseur> FilteredFournisseurs { get; } = new();

    [ObservableProperty] private bool    _isLoading;
    [ObservableProperty] private bool    _isFormVisible;
    [ObservableProperty] private string  _errorMessage = string.Empty;
    [ObservableProperty] private string? _searchQuery;

    [ObservableProperty] private string        _nomFournisseur       = string.Empty;
    [ObservableProperty] private string        _prenomFournisseur    = string.Empty;
    [ObservableProperty] private string        _telephoneFournisseur = string.Empty;
    [ObservableProperty] private Fournisseur?  _selectedFournisseur;

    public FournisseurViewModel(FournisseurServices fournisseurService)
    {
        _fournisseurService = fournisseurService;
    }

    [RelayCommand]
    private void NouveauFournisseur()
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
        if (string.IsNullOrWhiteSpace(NomFournisseur))
        {
            ErrorMessage = "Le nom du fournisseur est obligatoire.";
            return;
        }

        try
        {
            ErrorMessage = string.Empty;

            if (SelectedFournisseur is null)
                await _fournisseurService.AddFournisseur(NomFournisseur, PrenomFournisseur, TelephoneFournisseur);
            else
            {
                SelectedFournisseur.NomFournisseur    = NomFournisseur;
                SelectedFournisseur.PrenomFournisseur = PrenomFournisseur;
                SelectedFournisseur.TelephoneFournisseur = TelephoneFournisseur;
                await _fournisseurService.UpdateFournisseur(SelectedFournisseur);
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
    private async Task Supprimer(Fournisseur? fournisseur)
    {
        if (fournisseur is null) return;
        try
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            await _fournisseurService.DeleteFournisseur(fournisseur);
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.InnerException?.Message.Contains("foreign key constraint fails") == true ||
                           ex.Message.Contains("foreign key")
                ? "Impossible de supprimer : ce fournisseur est lié à des approvisionnements existants."
                : "Erreur lors de la suppression : " + ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void LoadFournisseurForEdit(Fournisseur fournisseur)
    {
        SelectedFournisseur   = fournisseur;
        NomFournisseur        = fournisseur.NomFournisseur;
        PrenomFournisseur     = fournisseur.PrenomFournisseur;
        TelephoneFournisseur  = fournisseur.TelephoneFournisseur;
        ErrorMessage          = string.Empty;
        IsFormVisible         = true;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            var data = await _fournisseurService.GetAllFournisseur();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Fournisseurs.Clear();
                FilteredFournisseurs.Clear();
                foreach (var f in data)
                {
                    Fournisseurs.Add(f);
                    FilteredFournisseurs.Add(f);
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
        SelectedFournisseur   = null;
        NomFournisseur        = string.Empty;
        PrenomFournisseur     = string.Empty;
        TelephoneFournisseur  = string.Empty;
        ErrorMessage          = string.Empty;
    }

    partial void OnSearchQueryChanged(string? value)
    {
        var query   = value ?? string.Empty;
        var results = string.IsNullOrWhiteSpace(query)
            ? Fournisseurs
            : Fournisseurs.Where(f =>
                (f.NomFournisseur?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (f.PrenomFournisseur?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (f.TelephoneFournisseur?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));

        FilteredFournisseurs.Clear();
        foreach (var item in results)
            FilteredFournisseurs.Add(item);
    }
}