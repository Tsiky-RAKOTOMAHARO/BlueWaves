using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class ApprovisionnementViewModel : ViewModelBase{
    [ObservableProperty]
    private int _idApp;

    [ObservableProperty]
    private int _refFournisseur;

    [ObservableProperty]
    private int _codeProduit;

    [ObservableProperty]
    private string? _certificat;
}