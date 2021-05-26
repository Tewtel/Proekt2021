// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.RoleBoolean
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal sealed class RoleBoolean : TrueFalseLiteral
  {
    private readonly MetadataItem m_metadataItem;

    internal RoleBoolean(EntitySetBase extent) => this.m_metadataItem = (MetadataItem) extent;

    internal RoleBoolean(AssociationSetEnd end) => this.m_metadataItem = (MetadataItem) end;

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      return (StringBuilder) null;
    }

    internal override DbExpression AsCqt(DbExpression row, bool skipIsNotNull) => (DbExpression) null;

    internal override StringBuilder AsUserString(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      if (this.m_metadataItem is AssociationSetEnd metadataItem)
        builder.Append(Strings.ViewGen_AssociationSet_AsUserString((object) blockAlias, (object) metadataItem.Name, (object) metadataItem.ParentAssociationSet));
      else
        builder.Append(Strings.ViewGen_EntitySet_AsUserString((object) blockAlias, (object) this.m_metadataItem.ToString()));
      return builder;
    }

    internal override StringBuilder AsNegatedUserString(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      if (this.m_metadataItem is AssociationSetEnd metadataItem)
        builder.Append(Strings.ViewGen_AssociationSet_AsUserString_Negated((object) blockAlias, (object) metadataItem.Name, (object) metadataItem.ParentAssociationSet));
      else
        builder.Append(Strings.ViewGen_EntitySet_AsUserString_Negated((object) blockAlias, (object) this.m_metadataItem.ToString()));
      return builder;
    }

    internal override void GetRequiredSlots(
      MemberProjectionIndex projectedSlotMap,
      bool[] requiredSlots)
    {
      throw new NotImplementedException();
    }

    protected override bool IsEqualTo(BoolLiteral right) => right is RoleBoolean roleBoolean && this.m_metadataItem == roleBoolean.m_metadataItem;

    public override int GetHashCode() => this.m_metadataItem.GetHashCode();

    internal override BoolLiteral RemapBool(Dictionary<MemberPath, MemberPath> remap) => (BoolLiteral) this;

    internal override void ToCompactString(StringBuilder builder)
    {
      if (this.m_metadataItem is AssociationSetEnd metadataItem)
        builder.Append("InEnd:" + metadataItem.ParentAssociationSet?.ToString() + "_" + metadataItem.Name);
      else
        builder.Append("InSet:" + this.m_metadataItem?.ToString());
    }
  }
}
