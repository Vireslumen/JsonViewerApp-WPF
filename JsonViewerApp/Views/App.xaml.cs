using System;
using System.Windows;
using JsonViewerApp.Factories;
using JsonViewerApp.Interfaces;
using JsonViewerApp.Services;
using JsonViewerApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace JsonViewerApp.Views;

/// <summary>
///     Логика взаимодействия для App.xaml
/// </summary>
public partial class App
{
    public IServiceProvider? ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Регистрация всех необходимых сервисов
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<IClipboardService, ClipboardService>();
        services.AddSingleton<IFileDialogService, FileDialogService>();
        services.AddSingleton<IProgressUpdater, ProgressUpdater>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IJsonParser, JsonParser>();
        services.AddSingleton<ISearchService, SearchService>();
        services.AddSingleton<ISpeedManager, SpeedManager>();
        services.AddSingleton<ISearchViewModelFactory, SearchViewModelFactory>();
        services.AddSingleton<MainViewModel>();
        services.AddTransient<MainWindow>();
    }
}