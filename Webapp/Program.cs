using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Eksamensprojekt_2.semester;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using WebApp.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<LoginService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AktivitetApiService>();
await builder.Build().RunAsync();

builder.Services.AddScoped<LoginService>();


await builder.Build().RunAsync();

static string GetApiBaseUrl(string? configuredApiBaseUrl, Uri webAppBaseUri)
{
    if (!string.IsNullOrWhiteSpace(configuredApiBaseUrl))
    {
        return configuredApiBaseUrl;
    }

    if (string.Equals(webAppBaseUri.Host, "localhost", StringComparison.OrdinalIgnoreCase))
    {
        return string.Equals(webAppBaseUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
            ? "https://localhost:7100/"
            : "http://localhost:5804/";
    }

    return webAppBaseUri.ToString();
}
