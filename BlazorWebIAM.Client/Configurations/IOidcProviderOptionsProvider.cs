// Changelogs Date  | Author                | Description
// 2023-02-22       | Anthony Coudène (ACE) | Creation


// Changelogs Date  | Author                | Description
// 2023-02-22       | Anthony Coudène (ACE) | Creation

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorWebIAM.Client.Configurations;

public interface IOidcProviderOptionsProvider
{

  /// <summary>
  /// Add options from tenant
  /// </summary>
  /// <param name="providerOptions"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  void ConfigureOptions(OidcProviderOptions providerOptions, RemoteAuthenticationUserOptions userOptions);
}
