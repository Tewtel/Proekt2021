// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.ExtentKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class ExtentKey : InternalBase
  {
    private readonly List<MemberPath> m_keyFields;

    internal ExtentKey(IEnumerable<MemberPath> keyFields) => this.m_keyFields = new List<MemberPath>(keyFields);

    internal IEnumerable<MemberPath> KeyFields => (IEnumerable<MemberPath>) this.m_keyFields;

    internal static List<ExtentKey> GetKeysForEntityType(
      MemberPath prefix,
      EntityType entityType)
    {
      return new List<ExtentKey>()
      {
        ExtentKey.GetPrimaryKeyForEntityType(prefix, entityType)
      };
    }

    internal static ExtentKey GetPrimaryKeyForEntityType(
      MemberPath prefix,
      EntityType entityType)
    {
      List<MemberPath> memberPathList = new List<MemberPath>();
      foreach (EdmMember keyMember in entityType.KeyMembers)
        memberPathList.Add(new MemberPath(prefix, keyMember));
      return new ExtentKey((IEnumerable<MemberPath>) memberPathList);
    }

    internal static ExtentKey GetKeyForRelationType(
      MemberPath prefix,
      AssociationType relationType)
    {
      List<MemberPath> memberPathList = new List<MemberPath>();
      foreach (AssociationEndMember associationEndMember in relationType.AssociationEndMembers)
      {
        ExtentKey keyForEntityType = ExtentKey.GetPrimaryKeyForEntityType(new MemberPath(prefix, (EdmMember) associationEndMember), MetadataHelper.GetEntityTypeForEnd(associationEndMember));
        memberPathList.AddRange(keyForEntityType.KeyFields);
      }
      return new ExtentKey((IEnumerable<MemberPath>) memberPathList);
    }

    internal string ToUserString() => StringUtil.ToCommaSeparatedStringSorted((IEnumerable) this.m_keyFields);

    internal override void ToCompactString(StringBuilder builder) => StringUtil.ToCommaSeparatedStringSorted(builder, (IEnumerable) this.m_keyFields);
  }
}
