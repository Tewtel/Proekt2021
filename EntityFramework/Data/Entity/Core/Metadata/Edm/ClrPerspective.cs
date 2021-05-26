// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ClrPerspective
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class ClrPerspective : Perspective
  {
    private EntityContainer _defaultContainer;

    internal ClrPerspective(MetadataWorkspace metadataWorkspace)
      : base(metadataWorkspace, DataSpace.CSpace)
    {
    }

    internal bool TryGetType(Type clrType, out TypeUsage outTypeUsage) => this.TryGetTypeByName(clrType.FullNameWithNesting(), false, out outTypeUsage);

    internal override bool TryGetMember(
      StructuralType type,
      string memberName,
      bool ignoreCase,
      out EdmMember outMember)
    {
      outMember = (EdmMember) null;
      MappingBase map = (MappingBase) null;
      if (this.MetadataWorkspace.TryGetMap((GlobalItem) type, DataSpace.OCSpace, out map) && map is ObjectTypeMapping objectTypeMapping)
      {
        ObjectMemberMapping memberMapForClrMember = objectTypeMapping.GetMemberMapForClrMember(memberName, ignoreCase);
        if (memberMapForClrMember != null)
        {
          outMember = memberMapForClrMember.EdmMember;
          return true;
        }
      }
      return false;
    }

    internal override bool TryGetTypeByName(
      string fullName,
      bool ignoreCase,
      out TypeUsage typeUsage)
    {
      typeUsage = (TypeUsage) null;
      MappingBase map = (MappingBase) null;
      if (this.MetadataWorkspace.TryGetMap(fullName, DataSpace.OSpace, ignoreCase, DataSpace.OCSpace, out map))
      {
        if (map.EdmItem.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType)
        {
          PrimitiveType mappedPrimitiveType = this.MetadataWorkspace.GetMappedPrimitiveType(((PrimitiveType) map.EdmItem).PrimitiveTypeKind, DataSpace.CSpace);
          if (mappedPrimitiveType != null)
            typeUsage = EdmProviderManifest.Instance.GetCanonicalModelTypeUsage(mappedPrimitiveType.PrimitiveTypeKind);
        }
        else
          typeUsage = ClrPerspective.GetMappedTypeUsage(map);
      }
      return typeUsage != null;
    }

    internal override EntityContainer GetDefaultContainer() => this._defaultContainer;

    internal void SetDefaultContainer(string defaultContainerName)
    {
      EntityContainer entityContainer = (EntityContainer) null;
      if (!string.IsNullOrEmpty(defaultContainerName) && !this.MetadataWorkspace.TryGetEntityContainer(defaultContainerName, DataSpace.CSpace, out entityContainer))
        throw new ArgumentException(Strings.ObjectContext_InvalidDefaultContainerName((object) defaultContainerName), nameof (defaultContainerName));
      this._defaultContainer = entityContainer;
    }

    private static TypeUsage GetMappedTypeUsage(MappingBase map)
    {
      TypeUsage typeUsage = (TypeUsage) null;
      if (map != null)
      {
        MetadataItem edmItem = map.EdmItem;
        EdmType edmType = edmItem as EdmType;
        if (edmItem != null && edmType != null)
          typeUsage = TypeUsage.Create(edmType);
      }
      return typeUsage;
    }
  }
}
