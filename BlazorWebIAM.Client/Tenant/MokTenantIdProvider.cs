namespace BlazorWebIAM.Client.Tenant;

public class MokTenantIdProvider : ITenantIdProvider
{
  const string TenantIdKey = "Tenant";

  public string? GetCurrentTenantId() => "devtenant";
  public string GetTenantIdKey() => TenantIdKey;  
}
