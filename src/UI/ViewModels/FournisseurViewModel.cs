using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Core.Models;
using Core.Services;
using System.Security.Cryptography.X509Certificates;

namespace UI.ViewModels;

public partial class FournisseurViewModel : ViewModelBase
{
    private readonly FournisseurServices _fournisseurService;

    public ObservableCollection<Fournisseur> Fournisseurs { get; } = new();

    [ObservableProperty] private bool    _isLoading;
    [ObservableProperty] private Fournisseur? _selectedFournisseur; 
    [ObservableProperty] private string  _nomFournisseur       = string.Empty;
    [ObservableProperty] private string  _prenomFournisseur    = string.Empty;
    [ObservableProperty] private string  _telephoneFournisseur = string.Empty;
    
    [ObservableProperty] private string  _errorMessage         = string.Empty;

    public FournisseurViewModel(FournisseurServices fournisseurService)
    {
        _fournisseurService = fournisseurService;
    }

    public async Task LoadFournisseur()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            Fournisseurs.Clear();
            var data = await _fournisseurService.GetAllFournisseur();
            foreach (var item in data)
                Fournisseurs.Add(item);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    public async Task SaveFournisseur()
    {
        try
        {
            ErrorMessage = string.Empty;
            await _fournisseurService.AddFournisseur(NomFournisseur, PrenomFournisseur, TelephoneFournisseur);
            await LoadFournisseur();
            ResetForm();
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

     public void LoadFournisseurForEdit(Fournisseur fournisseur){
            SelectedFournisseur = fournisseur;
            NomFournisseur = fournisseur.NomFournisseur;
            PrenomFournisseur = fournisseur.PrenomFournisseur;
            TelephoneFournisseur = fournisseur.TelephoneFournisseur;
            ErrorMessage = string.Empty;
        }

        public async Task RemoveFournisseur(Fournisseur fournisseur)
        {
            try
            {
                await _fournisseurService.DeleteFournisseur(fournisseur);
                Fournisseurs.Remove(fournisseur);
                await LoadFournisseur();
            }
            catch (ArgumentNullException ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    public void ResetForm()
    {
        NomFournisseur       = string.Empty;
        PrenomFournisseur    = string.Empty;
        TelephoneFournisseur = string.Empty;
        ErrorMessage         = string.Empty;
    }
}