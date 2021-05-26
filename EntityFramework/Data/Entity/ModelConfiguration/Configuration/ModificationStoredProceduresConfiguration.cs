// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ModificationStoredProceduresConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  internal class ModificationStoredProceduresConfiguration
  {
    private ModificationStoredProcedureConfiguration _insertModificationStoredProcedureConfiguration;
    private ModificationStoredProcedureConfiguration _updateModificationStoredProcedureConfiguration;
    private ModificationStoredProcedureConfiguration _deleteModificationStoredProcedureConfiguration;

    public ModificationStoredProceduresConfiguration()
    {
    }

    private ModificationStoredProceduresConfiguration(
      ModificationStoredProceduresConfiguration source)
    {
      if (source._insertModificationStoredProcedureConfiguration != null)
        this._insertModificationStoredProcedureConfiguration = source._insertModificationStoredProcedureConfiguration.Clone();
      if (source._updateModificationStoredProcedureConfiguration != null)
        this._updateModificationStoredProcedureConfiguration = source._updateModificationStoredProcedureConfiguration.Clone();
      if (source._deleteModificationStoredProcedureConfiguration == null)
        return;
      this._deleteModificationStoredProcedureConfiguration = source._deleteModificationStoredProcedureConfiguration.Clone();
    }

    public virtual ModificationStoredProceduresConfiguration Clone() => new ModificationStoredProceduresConfiguration(this);

    public virtual void Insert(
      ModificationStoredProcedureConfiguration modificationStoredProcedureConfiguration)
    {
      this._insertModificationStoredProcedureConfiguration = modificationStoredProcedureConfiguration;
    }

    public virtual void Update(
      ModificationStoredProcedureConfiguration modificationStoredProcedureConfiguration)
    {
      this._updateModificationStoredProcedureConfiguration = modificationStoredProcedureConfiguration;
    }

    public virtual void Delete(
      ModificationStoredProcedureConfiguration modificationStoredProcedureConfiguration)
    {
      this._deleteModificationStoredProcedureConfiguration = modificationStoredProcedureConfiguration;
    }

    public ModificationStoredProcedureConfiguration InsertModificationStoredProcedureConfiguration => this._insertModificationStoredProcedureConfiguration;

    public ModificationStoredProcedureConfiguration UpdateModificationStoredProcedureConfiguration => this._updateModificationStoredProcedureConfiguration;

    public ModificationStoredProcedureConfiguration DeleteModificationStoredProcedureConfiguration => this._deleteModificationStoredProcedureConfiguration;

    public virtual void Configure(
      EntityTypeModificationFunctionMapping modificationStoredProcedureMapping,
      DbProviderManifest providerManifest)
    {
      if (this._insertModificationStoredProcedureConfiguration != null)
        this._insertModificationStoredProcedureConfiguration.Configure(modificationStoredProcedureMapping.InsertFunctionMapping, providerManifest);
      if (this._updateModificationStoredProcedureConfiguration != null)
        this._updateModificationStoredProcedureConfiguration.Configure(modificationStoredProcedureMapping.UpdateFunctionMapping, providerManifest);
      if (this._deleteModificationStoredProcedureConfiguration == null)
        return;
      this._deleteModificationStoredProcedureConfiguration.Configure(modificationStoredProcedureMapping.DeleteFunctionMapping, providerManifest);
    }

    public void Configure(
      AssociationSetModificationFunctionMapping modificationStoredProcedureMapping,
      DbProviderManifest providerManifest)
    {
      if (this._insertModificationStoredProcedureConfiguration != null)
        this._insertModificationStoredProcedureConfiguration.Configure(modificationStoredProcedureMapping.InsertFunctionMapping, providerManifest);
      if (this._deleteModificationStoredProcedureConfiguration == null)
        return;
      this._deleteModificationStoredProcedureConfiguration.Configure(modificationStoredProcedureMapping.DeleteFunctionMapping, providerManifest);
    }

    public bool IsCompatibleWith(ModificationStoredProceduresConfiguration other) => (this._insertModificationStoredProcedureConfiguration == null || other._insertModificationStoredProcedureConfiguration == null || this._insertModificationStoredProcedureConfiguration.IsCompatibleWith(other._insertModificationStoredProcedureConfiguration)) && (this._deleteModificationStoredProcedureConfiguration == null || other._deleteModificationStoredProcedureConfiguration == null || this._deleteModificationStoredProcedureConfiguration.IsCompatibleWith(other._deleteModificationStoredProcedureConfiguration));

    public void Merge(
      ModificationStoredProceduresConfiguration modificationStoredProceduresConfiguration,
      bool allowOverride)
    {
      if (this._insertModificationStoredProcedureConfiguration == null)
        this._insertModificationStoredProcedureConfiguration = modificationStoredProceduresConfiguration.InsertModificationStoredProcedureConfiguration;
      else if (modificationStoredProceduresConfiguration.InsertModificationStoredProcedureConfiguration != null)
        this._insertModificationStoredProcedureConfiguration.Merge(modificationStoredProceduresConfiguration.InsertModificationStoredProcedureConfiguration, allowOverride);
      if (this._updateModificationStoredProcedureConfiguration == null)
        this._updateModificationStoredProcedureConfiguration = modificationStoredProceduresConfiguration.UpdateModificationStoredProcedureConfiguration;
      else if (modificationStoredProceduresConfiguration.UpdateModificationStoredProcedureConfiguration != null)
        this._updateModificationStoredProcedureConfiguration.Merge(modificationStoredProceduresConfiguration.UpdateModificationStoredProcedureConfiguration, allowOverride);
      if (this._deleteModificationStoredProcedureConfiguration == null)
      {
        this._deleteModificationStoredProcedureConfiguration = modificationStoredProceduresConfiguration.DeleteModificationStoredProcedureConfiguration;
      }
      else
      {
        if (modificationStoredProceduresConfiguration.DeleteModificationStoredProcedureConfiguration == null)
          return;
        this._deleteModificationStoredProcedureConfiguration.Merge(modificationStoredProceduresConfiguration.DeleteModificationStoredProcedureConfiguration, allowOverride);
      }
    }
  }
}
