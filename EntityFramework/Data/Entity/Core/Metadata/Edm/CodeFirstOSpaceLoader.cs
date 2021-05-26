﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.CodeFirstOSpaceLoader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class CodeFirstOSpaceLoader
  {
    private readonly CodeFirstOSpaceTypeFactory _typeFactory;

    public CodeFirstOSpaceLoader(CodeFirstOSpaceTypeFactory typeFactory = null) => this._typeFactory = typeFactory ?? new CodeFirstOSpaceTypeFactory();

    public void LoadTypes(
      EdmItemCollection edmItemCollection,
      ObjectItemCollection objectItemCollection)
    {
      foreach (EdmType edmType in edmItemCollection.OfType<EdmType>().Where<EdmType>((Func<EdmType, bool>) (t => t.BuiltInTypeKind == BuiltInTypeKind.EntityType || t.BuiltInTypeKind == BuiltInTypeKind.EnumType || t.BuiltInTypeKind == BuiltInTypeKind.ComplexType)))
      {
        Type clrType = edmType.GetClrType();
        if (clrType != (Type) null)
        {
          EdmType type = this._typeFactory.TryCreateType(clrType, edmType);
          if (type != null)
            this._typeFactory.CspaceToOspace.Add(edmType, type);
        }
      }
      this._typeFactory.CreateRelationships(edmItemCollection);
      foreach (Action referenceResolution in this._typeFactory.ReferenceResolutions)
        referenceResolution();
      foreach (MetadataItem metadataItem in this._typeFactory.LoadedTypes.Values)
        metadataItem.SetReadOnly();
      objectItemCollection.AddLoadedTypes(this._typeFactory.LoadedTypes);
      objectItemCollection.OSpaceTypesLoaded = true;
    }
  }
}
