using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingPOC.EmailService;
using TradingPOC.EmailService.Model.Settings;
using TradingPOC.EmailService.Service;

var config = new ConfigurationBuilder()
                         .AddJsonFile($"appsettings.json", optional: false)
                         .Build();
var appSettings = config.GetSection("AppSettings").Get<AppSettings>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<RabbitMQSettings>(appSettings.RabbitMQSettings);
        services.AddTransient<IRequestService,  RequestService>();
        services.AddHostedService<Worker>();
    }).Build();

await host.RunAsync();