using InvoiceAssistantV2.Client;
using InvoiceAssistantV2.Client.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace InvoiceAssistantV2.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // load the appsettings.json file to allow it to be used for dependency injection
            builder.Services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                IConfigurationSection section = config.GetSection("AppSettings");

                AppSettings appSettings = config.GetSection("AppSettings").Get<AppSettings>();
                return appSettings;

            });

            // A class that can be accessed through out the lifecycle of the application.
            // Used to pass data between pages.
            builder.Services.AddSingleton(provider =>
            {
                return new ApplicationSharedData();
            });


            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Create a scoped HttpClient to use in razor pages (via @inject HttpClient Http)
            // that sets the base url baseURL. This makes it so 
            // we don't have to provide the full url everytime we make a request to the server
            //services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                IConfigurationSection section = config.GetSection("AppSettings");
                AppSettings appSettings = config.GetSection("AppSettings").Get<AppSettings>();

                return new HttpClient
                {
                    BaseAddress = new Uri(appSettings.apiPostBackURL),
                };
            });

            await builder.Build().RunAsync();
        }
    }
}