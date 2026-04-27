using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Core.Interfaces;
using Core.Models;

namespace UI.ViewModels;

public class PurchaseOrderViewModel : ViewModelBase{
    public ApprovisionnementViewModel ApprovisionnementVM { get; }
    public CommandeViewModel CommandeVM { get; }

    public PurchaseOrderViewModel(
        ApprovisionnementViewModel approvisionnementVM,
        CommandeViewModel commandeVM)
    {
        ApprovisionnementVM = approvisionnementVM;
        CommandeVM = commandeVM;
    }

    public async Task InitializeAsync()
    {
        await ApprovisionnementVM.LoadDataAsync();
        await CommandeVM.LoadDataAsync();
    }
}