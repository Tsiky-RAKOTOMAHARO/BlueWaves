using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class StockViewModel : ViewModelBase{
    [ObservableProperty]
    private int _numeroStock;

    [ObservableProperty]
    private string? _type;
}