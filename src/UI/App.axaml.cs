using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
        var viewModel = Program.ServiceHost?.Services.GetRequiredService<ClientViewModel>();
     
        desktop.MainWindow = new MainWindow
        {
            DataContext = viewModel
        };
    }

    base.OnFrameworkInitializationCompleted();
}
}