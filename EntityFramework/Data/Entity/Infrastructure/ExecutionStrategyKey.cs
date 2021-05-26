// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.ExecutionStrategyKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// A key used for resolving <see cref="T:System.Func`1" />. It consists of the ADO.NET provider invariant name
  /// and the database server name as specified in the connection string.
  /// </summary>
  public class ExecutionStrategyKey
  {
    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Infrastructure.ExecutionStrategyKey" />
    /// </summary>
    /// <param name="providerInvariantName">
    /// The ADO.NET provider invariant name indicating the type of ADO.NET connection for which this execution strategy will be used.
    /// </param>
    /// <param name="serverName"> A string that will be matched against the server name in the connection string. </param>
    public ExecutionStrategyKey(string providerInvariantName, string serverName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(providerInvariantName, nameof (providerInvariantName));
      this.ProviderInvariantName = providerInvariantName;
      this.ServerName = serverName;
    }

    /// <summary>
    /// The ADO.NET provider invariant name indicating the type of ADO.NET connection for which this execution strategy will be used.
    /// </summary>
    public string ProviderInvariantName { get; private set; }

    /// <summary>
    /// A string that will be matched against the server name in the connection string.
    /// </summary>
    public string ServerName { get; private set; }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (!(obj is ExecutionStrategyKey executionStrategyKey) || !this.ProviderInvariantName.Equals(executionStrategyKey.ProviderInvariantName, StringComparison.Ordinal))
        return false;
      if (this.ServerName == null && executionStrategyKey.ServerName == null)
        return true;
      return this.ServerName != null && this.ServerName.Equals(executionStrategyKey.ServerName, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override int GetHashCode() => this.ServerName != null ? this.ProviderInvariantName.GetHashCode() ^ this.ServerName.GetHashCode() : this.ProviderInvariantName.GetHashCode();
  }
}
