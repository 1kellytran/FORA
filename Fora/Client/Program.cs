using Fora.Client;
using Fora.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IDataManager, DataManager>();
builder.Services.AddBlazoredLocalStorage();

//builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
