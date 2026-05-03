using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using UI.ViewModels;

namespace UI.Views;

public partial class PurchaseOrderView : UserControl
{
    public ApprovisionnementViewModel ApproVM    { get; }
    public CommandeViewModel          CommandeVM { get; }

    public PurchaseOrderView()
    {
        var s  = Program.ServiceHost!.Services;
        ApproVM    = s.GetRequiredService<ApprovisionnementViewModel>();
        CommandeVM = s.GetRequiredService<CommandeViewModel>();

        DataContext = this;
        InitializeComponent();
    }

    protected override async void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        await ApproVM.LoadDataAsync();
        await CommandeVM.LoadDataAsync();
    }

    private void OnApproCardClick(object? sender, PointerPressedEventArgs e) { }

    private void OnApproActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn) btn.ContextMenu?.Open(btn);
    }

    private void OnDeleteApproClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: Approvisionnement appro })
            ApproVM.SupprimerCommand.Execute(appro);
    }

    private void OnCommandeCardClick(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border { DataContext: Commande c })
            CommandeVM.SelectedCommande = c;
    }

    private void OnCommandeActionsClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: Commande c } btn)
        {
            CommandeVM.SelectedCommande = c;
            btn.ContextMenu?.Open(btn);
        }
    }

    private void OnCommandeDeleteClick(object? sender, RoutedEventArgs e)
    {
        if (CommandeVM.SelectedCommande is { } c)
            CommandeVM.DeleteCommandeCommand.Execute(c);
    }

    private void OnUpdateCommandeClick(object? sender, RoutedEventArgs e)
    {
        if (CommandeVM.SelectedCommande is not null)
            CommandeVM.UpdateCommandeCommand.Execute(null);
    }
}