// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ObjectTypeMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Mapping
{
  internal class ObjectTypeMapping : MappingBase
  {
    private readonly EdmType m_clrType;
    private readonly EdmType m_cdmType;
    private readonly string identity;
    private readonly Dictionary<string, ObjectMemberMapping> m_memberMapping;
    private static readonly Dictionary<string, ObjectMemberMapping> EmptyMemberMapping = new Dictionary<string, ObjectMemberMapping>(0);

    internal ObjectTypeMapping(EdmType clrType, EdmType cdmType)
    {
      this.m_clrType = clrType;
      this.m_cdmType = cdmType;
      this.identity = clrType.Identity + ":" + cdmType.Identity;
      if (Helper.IsStructuralType(cdmType))
        this.m_memberMapping = new Dictionary<string, ObjectMemberMapping>(((StructuralType) cdmType).Members.Count);
      else
        this.m_memberMapping = ObjectTypeMapping.EmptyMemberMapping;
    }

    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.MetadataItem;

    internal EdmType ClrType => this.m_clrType;

    internal override MetadataItem EdmItem => (MetadataItem) this.EdmType;

    internal EdmType EdmType => this.m_cdmType;

    internal override string Identity => this.identity;

    internal ObjectPropertyMapping GetPropertyMap(string propertyName)
    {
      ObjectMemberMapping memberMap = this.GetMemberMap(propertyName, false);
      return memberMap != null && (memberMap.MemberMappingKind == MemberMappingKind.ScalarPropertyMapping || memberMap.MemberMappingKind == MemberMappingKind.ComplexPropertyMapping) ? (ObjectPropertyMapping) memberMap : (ObjectPropertyMapping) null;
    }

    internal void AddMemberMap(ObjectMemberMapping memberMapping) => this.m_memberMapping.Add(memberMapping.EdmMember.Name, memberMapping);

    internal ObjectMemberMapping GetMemberMapForClrMember(
      string clrMemberName,
      bool ignoreCase)
    {
      return this.GetMemberMap(clrMemberName, ignoreCase);
    }

    private ObjectMemberMapping GetMemberMap(
      string propertyName,
      bool ignoreCase)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      ObjectMemberMapping objectMemberMapping = (ObjectMemberMapping) null;
      if (!ignoreCase)
      {
        this.m_memberMapping.TryGetValue(propertyName, out objectMemberMapping);
      }
      else
      {
        foreach (KeyValuePair<string, ObjectMemberMapping> keyValuePair in this.m_memberMapping)
        {
          if (keyValuePair.Key.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
            objectMemberMapping = objectMemberMapping == null ? keyValuePair.Value : throw new MappingException(Strings.Mapping_Duplicate_PropertyMap_CaseInsensitive((object) propertyName));
        }
      }
      return objectMemberMapping;
    }

    public override string ToString() => this.Identity;
  }
}
