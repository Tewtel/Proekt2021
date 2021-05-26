// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.NamedDbProviderService
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class NamedDbProviderService
  {
    private readonly string _invariantName;
    private readonly DbProviderServices _providerServices;

    public NamedDbProviderService(string invariantName, DbProviderServices providerServices)
    {
      this._invariantName = invariantName;
      this._providerServices = providerServices;
    }

    public string InvariantName => this._invariantName;

    public DbProviderServices ProviderServices => this._providerServices;
  }
}
