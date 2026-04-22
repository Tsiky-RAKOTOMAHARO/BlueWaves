using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class ExportViewModel : ViewModelBase{
    [ObservableProperty]
    private int _numeroExport;

    [ObservableProperty]
    private int _delai;
}