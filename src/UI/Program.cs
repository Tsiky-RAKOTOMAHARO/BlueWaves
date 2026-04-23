using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Repositories;
using Core.Interfaces;
using UI.ViewModels;
using System.IO;

namespace UI;

class Program
{
    // Propriété statique pour accéder aux services partout dans l'UI
    public static IHost? ServiceHost { get; private set; }

    [STAThread]
    public static void Main(string[] args)
    {
       
        ServiceHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                
                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

                
                services.AddScoped<IAchatRepository, AchatRepository>();
                services.AddScoped<IApprovisionnementRepository, ApprovisionnementRepository>();
                services.AddScoped<IClientRepository, ClientRepository>();
                services.AddScoped<ICommandeRepository, CommandeRepository>();
                services.AddScoped<IExportRepository, ExportRepository>();
                services.AddScoped<IFournisseurRepository, FournisseurRepository>();
                services.AddScoped<IProduitRepository, ProduitRepository>();
                services.AddScoped<IStockRepository, StockRepository>();

                // Enregistrement des ViewModels
                services.AddTransient<AchatViewModel>();
                services.AddTransient<ApprovisionnementViewModel>();
                services.AddTransient<ClientViewModel>();
                services.AddTransient<CommandeViewModel>();
                services.AddTransient<ExportViewModel>();
                services.AddTransient<FournisseurViewModel>();
                services.AddTransient<ProduitViewModel>();
                services.AddTransient<StockViewModel>();
            })
            .Build();

        
        ServiceHost.Start();

        
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}