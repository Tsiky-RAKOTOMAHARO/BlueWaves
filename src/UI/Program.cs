using System;
using System.IO;
using System.Threading.Tasks; // Nécessaire pour Task
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Repositories;
using Core.Interfaces;
using Core.Models;
using Core.Services;
using UI.ViewModels;

namespace UI;

class Program
{
    public static IHost? ServiceHost { get; private set; }

    [STAThread]
    public static async Task Main(string[] args)
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
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 35)); 

                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(connectionString, serverVersion));


                // Repositories
                services.AddSingleton<IAchatRepository, AchatRepository>();
                services.AddSingleton<IApprovisionnementRepository, ApprovisionnementRepository>();
                services.AddSingleton<IClientRepository, ClientRepository>();
                services.AddSingleton<ICommandeRepository, CommandeRepository>();
                services.AddSingleton<IExportRepository, ExportRepository>();
                services.AddSingleton<IFournisseurRepository, FournisseurRepository>();
                services.AddSingleton<IProduitRepository, ProduitRepository>();
                services.AddSingleton<IStockRepository, StockRepository>();// Corrigé ici

                // Services
                services.AddSingleton<ClientServices>();    
                services.AddSingleton<FournisseurServices>();    
                services.AddSingleton<ProduitServices>();
                services.AddSingleton<StockServices>();

                // ViewModels
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

        await ServiceHost.StartAsync();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseSkia();
}