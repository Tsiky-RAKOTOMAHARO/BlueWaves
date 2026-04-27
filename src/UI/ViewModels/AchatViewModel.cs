using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Core.Interfaces;
using Core.Models;

namespace UI.ViewModels;

public partial class AchatViewModel : ViewModelBase
{
    private readonly IAchatRepository _achatRepository;

    
    public ObservableCollection<Achat> Achats { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    
    [ObservableProperty] private int _idAchat;
    [ObservableProperty] private int _codeProduit;
    [ObservableProperty] private int _numeroCommande;
    [ObservableProperty] private int _quantite;

    public AchatViewModel(IAchatRepository achatRepository)
    {
        _achatRepository = achatRepository;
    }

    
    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Achats.Clear();

            var data = await _achatRepository.GetAllAchat();
            
            foreach (var item in data)
            {
                Achats.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    internal async Task LoadAchats()
    {
        throw new NotImplementedException();
    }

    internal void FilterAchats(string query){
        throw new NotImplementedException();
    }
}