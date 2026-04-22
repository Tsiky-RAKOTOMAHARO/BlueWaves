using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class AchatViewModel : ViewModelBase{
    [ObservableProperty]
    private int _idAchat;

    [ObservableProperty]
    private int _codeProduit;

    [ObservableProperty]
    private int _numeroCommande;

    [ObservableProperty]
    private int _quantite;
}

