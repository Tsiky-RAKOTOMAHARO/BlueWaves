using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class FournisseurViewModel : ViewModelBase{
    [ObservableProperty]
    private int _refFournisseur;

    [ObservableProperty]
    private string? _nomFournisseur;

    [ObservableProperty]
    private string? _prenomsFournisseur;

    [ObservableProperty]
    private string? _telephoneFournisseur;
}