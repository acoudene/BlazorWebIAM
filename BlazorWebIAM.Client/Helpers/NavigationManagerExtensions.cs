﻿// Changelogs Date  | Author                | Description
// 2022-11-22       | Anthony Coudène (ACE) | Creation

using Microsoft.AspNetCore.Components;

namespace BlazorWebIAM.Client.Helpers;

/// <summary>
/// Helper to enrich NavigationManager
/// </summary>
public static class NavigationManagerExtensions
{
  /// <summary>
  /// Try to get a value from a given key in query string
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="navManager"></param>
  /// <param name="key"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T? value)
  {
    var uri = navManager.ToAbsoluteUri(navManager.Uri);
    return uri.TryGetQueryString(key, out value);
  }
}
