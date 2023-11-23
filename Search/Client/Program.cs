using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.Service;
using Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<ClientSearchService, ClientSearchService>();
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(Config.LOADBALANCER_ADDRESS) });

await builder.Build().RunAsync();
