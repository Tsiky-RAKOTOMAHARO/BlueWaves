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
                options.UseMySql(connectionString, serverVersion),
                ServiceLifetime.Transient);

            // Repositories 
            services.AddTransient<IAchatRepository, AchatRepository>();
            services.AddTransient<IApprovisionnementRepository, ApprovisionnementRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<ICommandeRepository, CommandeRepository>();
            services.AddTransient<IExportRepository, ExportRepository>();
            services.AddTransient<IFournisseurRepository, FournisseurRepository>();
            services.AddTransient<IProduitRepository, ProduitRepository>();
            services.AddTransient<IStockRepository, StockRepository>();

            // Services 
            services.AddTransient<ClientServices>();
            services.AddTransient<FournisseurServices>();
            services.AddTransient<ProduitServices>();
            services.AddTransient<StockServices>();
            services.AddTransient<CommandeServices>();
            services.AddTransient<ApprovisionnementServices>();

            // ViewModels 
            services.AddSingleton<ApprovisionnementViewModel>();
            services.AddSingleton<CommandeViewModel>();
            services.AddSingleton<ClientViewModel>();
            services.AddSingleton<FournisseurViewModel>();
            services.AddSingleton<ProduitViewModel>();
            services.AddSingleton<StockViewModel>();
            services.AddSingleton<ExportViewModel>();
            services.AddSingleton<AchatViewModel>();

                
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