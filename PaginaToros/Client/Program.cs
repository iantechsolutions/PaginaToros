using Blazored.Modal;
using Blazored.SessionStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PaginaToros.Client;
using PaginaToros.Client.Auth;
using PaginaToros.Client.Servicios.Contrato;
using PaginaToros.Client.Servicios.Implementacion;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredModal();
builder.Services.AddSweetAlert2();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<JwtAuthenticatorProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticatorProvider>(provider => provider.GetRequiredService<JwtAuthenticatorProvider>());
builder.Services.AddScoped<ILoginServices, JwtAuthenticatorProvider>(provider => provider.GetRequiredService<JwtAuthenticatorProvider>());
builder.Services.AddScoped<ICentrosiumServicio, CentrosiumServicio>();
builder.Services.AddScoped<ICertifsemanServicio, CertifsemanServicio>();
builder.Services.AddScoped<IDesepla1Servicio, Desepla1Servicio>();
builder.Services.AddScoped<IDesepla3Servicio, Desepla3Servicio>();
builder.Services.AddScoped<IEstableServicio, EstableServicio>();
builder.Services.AddScoped<IFutcontrolServicio, FutcontrolServicio>();
builder.Services.AddScoped<IInspectServicio, InspectServicio>();
builder.Services.AddScoped<IPlantelServicio, PlantelServicio>();
builder.Services.AddScoped<IResin1Servicio, Resin1Servicio>();
builder.Services.AddScoped<IResin2Servicio, Resin2Servicio>();
builder.Services.AddScoped<IResin3Servicio, Resin3Servicio>();
builder.Services.AddScoped<IResin4Servicio, Resin4Servicio>();
builder.Services.AddScoped<IResin6Servicio, Resin6Servicio>();
builder.Services.AddScoped<IResin8Servicio, Resin8Servicio>();
builder.Services.AddScoped<ISocioServicio, SocioServicio>();
builder.Services.AddScoped<ISolici1Servicio, Solici1Servicio>();
builder.Services.AddScoped<ISolici1AuxServicio, Solici1AuxServicio>();
builder.Services.AddScoped<ITorosServicio, TorosServicio>();
builder.Services.AddScoped<ITransanServicio, TransanServicio>();
builder.Services.AddScoped<ITranssbServicio, TranssbServicio>();

await builder.Build().RunAsync();


//using Blazored.Modal;
//using Blazored.SessionStorage;
//using CurrieTechnologies.Razor.SweetAlert2;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using PaginaToros.Client;
//using Radzen;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddHttpClient();
//builder.Services.AddAuthorizationCore();
//builder.Services.AddBlazoredSessionStorage();
//builder.Services.AddBlazoredModal();
//builder.Services.AddSweetAlert2();
//builder.Services.AddScoped<DialogService>();
//builder.Services.AddScoped<NotificationService>();
//builder.Services.AddScoped<TooltipService>();
//builder.Services.AddScoped<ContextMenuService>();

//await builder.Build().RunAsync();
