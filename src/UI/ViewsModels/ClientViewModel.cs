using CommunityToolkit.Mvvm.ComponentModel;

namespace UI.ViewModels;

public partial class ClientViewModel : ViewModelBase{
    
    [ObservableProperty]
    private int _refClient;

    [ObservableProperty]
    private string? _nomClient;

    [ObservableProperty]
    private string? _prenomClient;

    [ObservableProperty]
    private string? _telephone;

    // Note : On ne met pas la collection de Commandes ici pour l'instant. 
    // On l'ajoutera seulement si la vue "Client" a besoin d'afficher l'historique de ses commandes.
}