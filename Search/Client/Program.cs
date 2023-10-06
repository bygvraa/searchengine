using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.Service;
using Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<AppDataService, AppDataService>();
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(Config.APIADDRESS) });

await builder.Build().RunAsync();
