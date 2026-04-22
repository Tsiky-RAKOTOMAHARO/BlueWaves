using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class ProduitViewModel : ViewModelBase{
    [ObservableProperty]
    private int _codeProduit;

    [ObservableProperty]
    private int _numeroStock;

    [ObservableProperty]
    private string? _nomProduit;

    [ObservableProperty]
    private int _quantite;

    [ObservableProperty]
    private DateTime _dateReception;

    [ObservableProperty]
    private bool _statut;
}