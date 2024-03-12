// Changelogs Date  | Author                | Description
// 2022-11-22       | Anthony Coudène (ACE) | Creation

using BlazorWebIAM.Client.Helpers;
using Microsoft.AspNetCore.Http.Extensions;

namespace BlazorWebIAM.Tenant;

/// <summary>
/// Stateless provider to manager tenant id by subdomain
/// </summary>
public class ByReqSubDomainTenantIdProvider : IStatefulTenantIdProvider
{
  private const string TenantIdKey = "Tenant";
  private readonly IHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// Get tenant key
  /// </summary>
  /// <returns></returns>
  public string GetTenantIdKey() => TenantIdKey;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="authenticationState"></param>
  /// <param name="claimsProvider"></param>
  public ByReqSubDomainTenantIdProvider(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
  }

  /// <summary>
  /// Get tenant id from current state
  /// </summary>
  /// <returns></returns>
  public string? GetCurrentTenantId()
  {
    var request = _httpContextAccessor.HttpContext?.Request;
    if (request is null)
      return null;

    var url = request.GetDisplayUrl();
    if (string.IsNullOrWhiteSpace(url))
      throw new InvalidOperationException("No display url for request");

    var uri = new Uri(url);
    return uri.GetSubdomain();
  }
}