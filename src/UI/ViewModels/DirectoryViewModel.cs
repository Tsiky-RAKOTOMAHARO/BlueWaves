using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Interfaces;
using Core.Models;

namespace UI.ViewModels;


public partial class DirectoryViewModel : ViewModelBase{
    public FournisseurViewModel FournisseurVM { get; }
    public ClientViewModel ClientVM { get; }

    public DirectoryViewModel(FournisseurViewModel fournisseurVM, ClientViewModel clientVM){

        FournisseurVM = fournisseurVM;
        ClientVM = clientVM;
    }
}
    