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
                services.AddScoped<IAchatRepository, AchatRepository>();
                services.AddScoped<IApprovisionnementRepository, ApprovisionnementRepository>();
                services.AddScoped<IClientRepository, ClientRepository>();
                services.AddScoped<ICommandeRepository, CommandeRepository>();
                services.AddScoped<IExportRepository, ExportRepository>();
                services.AddScoped<IFournisseurRepository, FournisseurRepository>();
                services.AddScoped<IProduitRepository, ProduitRepository>();
                services.AddScoped<IStockRepository, StockRepository>(); // Corrigé ici

                // Services
                services.AddScoped<ClientServices>();    
                services.AddScoped<FournisseurServices>();    

                // ViewModels
                services.AddTransient<AchatViewModel>();
                services.AddTransient<ApprovisionnementViewModel>();
                services.AddTransient<ClientViewModel>();
                services.AddTransient<CommandeViewModel>();
                services.AddTransient<ExportViewModel>();
                services.AddTransient<FournisseurViewModel>();
                services.AddTransient<ProduitViewModel>();
                services.AddTransient<StockViewModel>();

                services.AddScoped<Core.Services.ProduitServices>(); 
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