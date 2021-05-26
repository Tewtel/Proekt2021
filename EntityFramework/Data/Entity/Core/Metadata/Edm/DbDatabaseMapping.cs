// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.DbDatabaseMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class DbDatabaseMapping
  {
    private readonly List<EntityContainerMapping> _entityContainerMappings = new List<EntityContainerMapping>();

    public EdmModel Model { get; set; }

    public EdmModel Database { get; set; }

    public DbProviderInfo ProviderInfo => this.Database.ProviderInfo;

    public DbProviderManifest ProviderManifest => this.Database.ProviderManifest;

    internal IList<EntityContainerMapping> EntityContainerMappings => (IList<EntityContainerMapping>) this._entityContainerMappings;

    internal void AddEntityContainerMapping(EntityContainerMapping entityContainerMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityContainerMapping>(entityContainerMapping, nameof (entityContainerMapping));
      this._entityContainerMappings.Add(entityContainerMapping);
    }
  }
}
