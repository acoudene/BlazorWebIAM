// Changelogs Date  | Author                | Description
// 2022-11-22       | Anthony Coudène (ACE) | MN-1198-Creation

using BlazorWebIAM.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorWebIAM.Client.Tenant;

/// <summary>
/// Get a tenant id by query string
/// </summary>
public class ByNavQueryStringTenantIdProvider : ITenantIdProvider
{
  private const string TenantIdKey = "Tenant";

  private readonly NavigationManager _navigationManager;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="navigationManager"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public ByNavQueryStringTenantIdProvider(NavigationManager navigationManager)
  {    
    _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
  }

  /// <summary>
  /// Get tenant key
  /// </summary>
  /// <returns></returns>
  public string GetTenantIdKey() => TenantIdKey;

  /// <summary>
  /// Add or update an uri with Tenant
  /// </summary>
  /// <param name="uri"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public Uri AddOrUpdateTenant(Uri uri)
  {
    if (uri is null) throw new ArgumentNullException(nameof(uri));

    var tenantIdkeyName = GetTenantIdKey();
    if (string.IsNullOrWhiteSpace(tenantIdkeyName))
      throw new InvalidOperationException("Missing key name for tenant id value");

    string? expectedTenantId = GetCurrentTenantId();
    if (string.IsNullOrEmpty(expectedTenantId))
      throw new InvalidOperationException("No tenant is found");

    // If tenant parameter already given
    if (uri.TryGetQueryString<string>(tenantIdkeyName, out string? tenantId)
        && !string.IsNullOrWhiteSpace(tenantId)
        && tenantId.Equals(expectedTenantId))
      return uri;

    return AddOrReplaceQueryString(uri, tenantIdkeyName, expectedTenantId);
  }

  /// <summary>
  /// Add or update an uri string with Tenant
  /// </summary>
  /// <param name="uri"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public string AddOrUpdateTenant(string uri)
  {
    var newUri = new Uri(uri, UriKind.RelativeOrAbsolute);
    var uriWithTenant = AddOrUpdateTenant(newUri);
    if (uriWithTenant == null)
      throw new InvalidOperationException("Error while adding tenant to uri");

    return (uriWithTenant.IsAbsoluteUri ? uriWithTenant.AbsoluteUri : uriWithTenant.ToString());
  }

  private Uri AddOrReplaceQueryString(Uri uri, string key, string value)
  {
    if (uri is null) throw new ArgumentNullException(nameof(uri));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
    
    string selectedUri = uri.ToString();

    if (uri.IsAbsoluteUri)
    {
      var newUri = uri.RemoveQueryString(key);
      if (newUri != null)
      {
        selectedUri = newUri.AbsoluteUri;
      }
    }

    string newUriWithQueryString = QueryHelpers.AddQueryString(selectedUri, key, value);
    return new Uri(newUriWithQueryString, UriKind.RelativeOrAbsolute);
  }

  /// <summary>
  /// Get tenant id from current state
  /// </summary>
  /// <returns></returns>
  public string? GetCurrentTenantId()
  {
    var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
    if (uri == null)
      return _navigationManager.Uri;

    uri.TryGetQueryString<string?>(TenantIdKey, out string? tenantId);
    return tenantId;
  }
}
