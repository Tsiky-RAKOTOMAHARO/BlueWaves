using System;
using System.Linq;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using Microsoft.Extensions.DependencyInjection;
using Core.Models;
using UI.ViewModels;

namespace UI.Views;

public partial class DirectoryView : UserControl
{
    private ClientViewModel? _clientVM;
    private FournisseurViewModel? _fournisseurVM;

    public DirectoryView(){
        InitializeComponent();

        if (Program.ServiceHost != null){
            _clientVM = Program.ServiceHost.Services.GetRequiredService<ClientViewModel>();
            _fournisseurVM = Program.ServiceHost.Services.GetRequiredService<FournisseurViewModel>();

            DataContext = new { ClientVM = _clientVM, FournisseurVM = _fournisseurVM };

            this.AttachedToVisualTree += OnAttachedToVisualTree;
        }
    }

    private async void OnAttachedToVisualTree(object? sender, EventArgs e){
        if (_clientVM == null || _fournisseurVM == null) return;

        try{
            if (_clientVM.Clients.Count == 0)
                await _clientVM.LoadClients();

            if (_fournisseurVM.Fournisseurs.Count == 0)
                await _fournisseurVM.LoadFournisseur();
        }
        catch (Exception ex){
            Debug.WriteLine($"Error: {ex.Message}");
        }

        if (this.FindControl<Button>("SaveContactButton") is Button saveBtn){
            saveBtn.Click -= SaveContact_Click;
            saveBtn.Click += SaveContact_Click;
        }
    }

    private async void SaveContact_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        if (_clientVM == null || _fournisseurVM == null) return;

        var typeComboBox = this.FindControl<ComboBox>("TypeComboBox");
        var nomTextBox = this.FindControl<TextBox>("NomTextBox");
        var prenomTextBox = this.FindControl<TextBox>("PrenomTextBox");
        var telephoneTextBox = this.FindControl<TextBox>("TelephoneTextBox");

        if (typeComboBox == null || typeComboBox.SelectedIndex < 0) return;
        
        string nom = nomTextBox?.Text?.Trim() ?? string.Empty;
        string prenom = prenomTextBox?.Text?.Trim() ?? string.Empty;
        string telephone = telephoneTextBox?.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(telephone)) return;

        try
        {
            bool isClient = typeComboBox.SelectedIndex == 0;

            if (isClient)
            {
                var newClient = new Client 
                { 
                    NomClient = nom, 
                    PrenomClient = prenom, 
                    Telephone = telephone 
                };
                await _clientVM.SaveClient(newClient);
            }
            else
            {
                var newFournisseur = new Fournisseur 
                { 
                    NomFournisseur = nom, 
                    PrenomFournisseur = prenom, 
                    TelephoneFournisseur = telephone 
                };
                await _fournisseurVM.SaveFournisseur(newFournisseur);
            }

            ResetForm(nomTextBox, prenomTextBox, telephoneTextBox, typeComboBox);
            CloseParentFlyout(sender as Button);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    private void ResetForm(TextBox? nom, TextBox? prenom, TextBox? tel, ComboBox? type)
    {
        if (nom != null) nom.Text = string.Empty;
        if (prenom != null) prenom.Text = string.Empty;
        if (tel != null) tel.Text = string.Empty;
        if (type != null) type.SelectedIndex = 0;
    }

    private void CloseParentFlyout(Button? button)
    {
        if (button == null) return;

        var flyoutAnchor = button.GetVisualAncestors()
            .OfType<Button>()
            .FirstOrDefault(b => FlyoutBase.GetAttachedFlyout(b) != null);

        if (flyoutAnchor != null)
        {
            FlyoutBase.GetAttachedFlyout(flyoutAnchor)?.Hide();
        }
    }
}