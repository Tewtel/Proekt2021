// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbProviderInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Groups a pair of strings that identify a provider and server version together into a single object.
  /// </summary>
  /// <remarks>
  /// Instances of this class act as the key for resolving a <see cref="T:System.Data.Entity.Spatial.DbSpatialServices" /> for a specific
  /// provider from a <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" />. This is typically used when registering spatial services
  /// in <see cref="T:System.Data.Entity.DbConfiguration" /> or when the spatial services specific to a provider is
  /// resolved by an implementation of <see cref="T:System.Data.Entity.Core.Common.DbProviderServices" />.
  /// </remarks>
  public sealed class DbProviderInfo
  {
    private readonly string _providerInvariantName;
    private readonly string _providerManifestToken;

    /// <summary>
    /// Creates a new object for a given provider invariant name and manifest token.
    /// </summary>
    /// <param name="providerInvariantName">
    /// A string that identifies that provider. For example, the SQL Server
    /// provider uses the string "System.Data.SqlCient".
    /// </param>
    /// <param name="providerManifestToken">
    /// A string that identifies that version of the database server being used. For example, the SQL Server
    /// provider uses the string "2008" for SQL Server 2008. This cannot be null but may be empty.
    /// The manifest token is sometimes referred to as a version hint.
    /// </param>
    public DbProviderInfo(string providerInvariantName, string providerManifestToken)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(providerInvariantName, nameof (providerInvariantName));
      System.Data.Entity.Utilities.Check.NotNull<string>(providerManifestToken, nameof (providerManifestToken));
      this._providerInvariantName = providerInvariantName;
      this._providerManifestToken = providerManifestToken;
    }

    /// <summary>
    /// A string that identifies that provider. For example, the SQL Server
    /// provider uses the string "System.Data.SqlCient".
    /// </summary>
    public string ProviderInvariantName => this._providerInvariantName;

    /// <summary>
    /// A string that identifies that version of the database server being used. For example, the SQL Server
    /// provider uses the string "2008" for SQL Server 2008. This cannot be null but may be empty.
    /// </summary>
    public string ProviderManifestToken => this._providerManifestToken;

    private bool Equals(DbProviderInfo other) => string.Equals(this._providerInvariantName, other._providerInvariantName) && string.Equals(this._providerManifestToken, other._providerManifestToken);

    /// <inheritdoc />
    public override bool Equals(object obj) => obj is DbProviderInfo other && this.Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => this._providerInvariantName.GetHashCode() * 397 ^ this._providerManifestToken.GetHashCode();
  }
}
