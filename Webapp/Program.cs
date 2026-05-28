using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Eksamensprojekt_2.semester;
using Blazored.SessionStorage;
using WebApp.Service;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<DeltagerApiService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AktivitetApiService>();
builder.Services.AddScoped<TeamchatAPIService>();

await builder.Build().RunAsync();

