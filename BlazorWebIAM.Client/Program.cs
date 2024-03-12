using BlazorWebIAM.Client;
using BlazorWebIAM.Client.Configurations;
using BlazorWebIAM.Client.Security;
using BlazorWebIAM.Client.Tenant;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHost? host = null;
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddHttpClient("Authenticated", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Authenticated"));

builder.Services.AddScoped<ITenantIdProvider, MokTenantIdProvider>();
builder.Services.AddScoped<IOidcProviderOptionsProvider, ByTenantOidcProviderOptionsProvider>();

builder.Services
    .AddOidcAuthentication(options =>
    {
      if (host == null) throw new InvalidOperationException("Missing host");

      var oidcProviderOptionsProvider = host.Services.GetRequiredService<IOidcProviderOptionsProvider>();
      if (oidcProviderOptionsProvider == null)
        throw new InvalidOperationException($"Missing {nameof(IOidcProviderOptionsProvider)} implementation");

      oidcProviderOptionsProvider.ConfigureOptions(options.ProviderOptions, options.UserOptions);
    })
    .AddAccountClaimsPrincipalFactory<MyClaimsPrincipalFactory>();

builder.Services.AddApiAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

host = builder.Build();
await host.RunAsync();
