// See https://aka.ms/new-console-template for more information
using LunaSonar.Console.ServicesImpl;
using LunaSonar.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spectre.Console;
using Serilog.Sinks.Spectre;
using Serilog.Events;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

SetDisplay();
//var host = AppStartup();

//var asdf = ActivatorUtilities.CreateInstance<IVrChatApiService>(host.Services);

//asdf.Init();

CancellationTokenSource cancellationTokenSource = new();
CancellationToken cancellationToken = cancellationTokenSource.Token;
ConfigureCancellationOnUserRequest(cancellationTokenSource);
IHost? host = default;
host = AppStartup();
await host.StartAsync(cancellationToken).ConfigureAwait(false);

var logger = ActivatorUtilities.GetServiceOrCreateInstance<ILogger<Program>>(host.Services);
try
{
   

    // A scope is required so that Runner class can use scoped lifetime services.
    //await using (AsyncServiceScope scope = host.Services.CreateAsyncScope())
    //{
    //    // Main logic of the app is in "RunAsync" method of "Runner" instance.
    //    Runner runner = ActivatorUtilities.GetServiceOrCreateInstance<Runner>(scope.ServiceProvider);
    //    await runner.RunAsync(cancellationToken).ConfigureAwait(false);
    //}

    var vrChatApi = ActivatorUtilities.GetServiceOrCreateInstance<IVrChatApiService>(host.Services);
    var settings = ActivatorUtilities.GetServiceOrCreateInstance<ISettingsService>(host.Services);

    var settingsData = settings.Load();
    if (settingsData == null)
    {
        settingsData = new LunaSonar.Core.Models.SettingsModel();
    }
    if (string.IsNullOrWhiteSpace(settingsData.Token))
    {
        settingsData.UserName = AnsiConsole.Ask<string>("Username:", settingsData.UserName).Trim();

        settingsData.Password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").PromptStyle("red").Secret());
    }

   

    //settingsData.Password = AnsiConsole.Ask<string>("Password:", settingsData.Password).Trim();
    
    settings.Save(settingsData);

    await vrChatApi.Init();

    if (AnsiConsole.Confirm("Log in?"))
    {
        await vrChatApi.Authenticate();
        AnsiConsole.WriteLine("Logged in");

        settingsData = settings.Load();

        settingsData.SearchString = AnsiConsole.Ask<string>("Search string:", settingsData.SearchString).Trim().ToLower();
        settings.Save(settingsData);

        

        var t = new PeriodicTimer(TimeSpan.FromMinutes(1.2));
        while (await t.WaitForNextTickAsync(cancellationToken))
        {
            var res = await vrChatApi.Monitor();
            if (res.PresentNames.Any())
            {
                foreach (var item in res.PresentNames)
                {
                    logger.LogInformation($"Found: {item}");
                    //AnsiConsole.WriteLine();
                }
            }
            else
            {
                logger.LogInformation($"Nobody Found");
            }
           
        }

        

    }




    await host.WaitForShutdownAsync(cancellationToken).ConfigureAwait(false);
}
catch (TaskCanceledException)
{
    //AnsiConsole.Write("\n\n");
}
catch (Exception ex)
{
    logger.LogError(ex.Demystify(), "Error");
    //AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}
finally
{
    cancellationTokenSource?.Dispose();

    if (host is IAsyncDisposable disposableHost)
    {
        await disposableHost.DisposeAsync().ConfigureAwait(false);
    }

    //DisplayFarewell();
    await Task.Delay(500);
}

/// <summary>
/// When user presses Ctrl + C, cancelation will be requested. Methods should be ready to take this
/// request and throw a TaskCanceled exception that will stop the application.
/// </summary>
/// <param name="cancellationTokenSource">Global/Shared cancellation source for stopping the app</param>
static void ConfigureCancellationOnUserRequest(CancellationTokenSource cancellationTokenSource)
{
    Console.CancelKeyPress += (_, e) =>
    {
        cancellationTokenSource.Cancel();
        e.Cancel = true;
    };
}

static void BuildConfig(IConfigurationBuilder builder)
{
    // Check the current directory that the application is running on 
    // Then once the file 'appsetting.json' is found, we are adding it.
    // We add env variables, which can override the configs in appsettings.json
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}

static IHost AppStartup()
{
    var builder = new ConfigurationBuilder();
    BuildConfig(builder);

    // Specifying the configuration for serilog
    Log.Logger = new LoggerConfiguration() // initiate the logger configuration
                    .ReadFrom.Configuration(builder.Build()) // connect serilog to our configuration folder
                    .Enrich.FromLogContext() //Adds more information to our logs from built in Serilog 
                    .WriteTo.Debug()
                    .WriteTo.Spectre("{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}") // decide where the logs are going to be shown
                    .CreateLogger(); //initialise the logger

    Log.Logger.Information("Application Starting");

    var host = Host.CreateDefaultBuilder() // Initialising the Host 
                .ConfigureServices((context, services) =>
                { // Adding the DI container for configuration
                    services.AddSingleton<ISettingsService, ConsoleSettingsService>();
                    services.AddScoped<IVrChatApiService, VrChatApiService>();
                })
                .UseSerilog() // Add Serilog
                .Build(); // Build the Host

    return host;
}

/// <summary>
/// Configures the display.
/// </summary>
static void SetDisplay()
{
    AnsiConsole.Reset();
    AnsiConsole.Clear();
    AnsiConsole.Profile.Width = 800;
    AnsiConsole.Profile.Height = 600;
}
//Console.WriteLine("Hello, World!");

//var container = new ServiceCollection();
//container.AddSingleton<ISettingsService, ConsoleSettingsService>();
//container.AddScoped<IVrChatApiService, VrChatApiService>();

//var serviceProvider = container.BuildServiceProvider();

//var apiService = serviceProvider.GetService<IVrChatApiService>();


