using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class CommandeViewModel : ViewModelBase{
    [ObservableProperty]
    private int _numeroCommande;

    [ObservableProperty]
    private int _refClient;

    [ObservableProperty]
    private int _codeExport;

    [ObservableProperty]
    private DateTime _dateCommande;

    [ObservableProperty]
    private string? _destination;
}