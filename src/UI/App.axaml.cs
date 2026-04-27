using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Core.Services;
using UI.ViewModels;
using UI.Views;

namespace UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel       = Program.ServiceHost?.Services.GetRequiredService<ClientViewModel>();
            var produitServices = Program.ServiceHost?.Services.GetRequiredService<Core.Services.ProduitServices>();

            desktop.MainWindow = new MainWindow(produitServices!)
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}