using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Core.Interfaces;
using Core.Models;

namespace UI.ViewModels;

public partial class FournisseurViewModel : ViewModelBase
{
    private readonly IFournisseurRepository _repository;

    public ObservableCollection<Fournisseur> Fournisseurs { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _refFournisseur;
    [ObservableProperty] private string? _nomFournisseur;
    [ObservableProperty] private string? _prenomFournisseur;
    [ObservableProperty] private string? _telephoneFournisseur;
    [ObservableProperty] private int _selectedTypeIndex = 0;

    public FournisseurViewModel(IFournisseurRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadFournisseur()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Fournisseurs.Clear();

            var data = await _repository.GetAllFournisseur();
            
            foreach (var item in data)
            {
                Fournisseurs.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task SaveFournisseur(Fournisseur newFournisseur)
    {
        if (string.IsNullOrWhiteSpace(NomFournisseur) || string.IsNullOrWhiteSpace(TelephoneFournisseur))
        {
            return;
        }

        var fournisseur = new Fournisseur
        {
            NomFournisseur = NomFournisseur,
            PrenomFournisseur = PrenomFournisseur ?? string.Empty,
            TelephoneFournisseur = TelephoneFournisseur
        };

        try
        {
            if (RefFournisseur == 0)
            {
                await _repository.AddFournisseur(fournisseur);
            }
            else
            {
                fournisseur.RefFournisseur = RefFournisseur;
                await _repository.UpdateFournisseur(fournisseur);
            }

            // Réinitialiser les champs
            ClearForm();
            await LoadFournisseur();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors de la sauvegarde: {ex.Message}");
        }
    }

    public void ClearForm()
    {
        RefFournisseur = 0;
        NomFournisseur = null;
        PrenomFournisseur = null;
        TelephoneFournisseur = null;
        SelectedTypeIndex = 0;
    }
}