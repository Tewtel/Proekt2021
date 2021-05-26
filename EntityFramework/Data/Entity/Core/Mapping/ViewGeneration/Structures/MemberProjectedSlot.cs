﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.MemberProjectedSlot
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal sealed class MemberProjectedSlot : ProjectedSlot
  {
    private readonly MemberPath m_memberPath;

    internal MemberProjectedSlot(MemberPath node) => this.m_memberPath = node;

    internal MemberPath MemberPath => this.m_memberPath;

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      MemberPath outputMember,
      string blockAlias,
      int indentLevel)
    {
      TypeUsage outputMemberTypeUsage;
      if (this.NeedToCastCqlValue(outputMember, out outputMemberTypeUsage))
      {
        builder.Append("CAST(");
        this.m_memberPath.AsEsql(builder, blockAlias);
        builder.Append(" AS ");
        CqlWriter.AppendEscapedTypeName(builder, outputMemberTypeUsage.EdmType);
        builder.Append(')');
      }
      else
        this.m_memberPath.AsEsql(builder, blockAlias);
      return builder;
    }

    internal override DbExpression AsCqt(DbExpression row, MemberPath outputMember)
    {
      DbExpression dbExpression = this.m_memberPath.AsCqt(row);
      TypeUsage outputMemberTypeUsage;
      if (this.NeedToCastCqlValue(outputMember, out outputMemberTypeUsage))
        dbExpression = (DbExpression) dbExpression.CastTo(outputMemberTypeUsage);
      return dbExpression;
    }

    private bool NeedToCastCqlValue(MemberPath outputMember, out TypeUsage outputMemberTypeUsage)
    {
      TypeUsage modelTypeUsage = Helper.GetModelTypeUsage(this.m_memberPath.LeafEdmMember);
      outputMemberTypeUsage = Helper.GetModelTypeUsage(outputMember.LeafEdmMember);
      return !modelTypeUsage.EdmType.Equals((object) outputMemberTypeUsage.EdmType);
    }

    internal override void ToCompactString(StringBuilder builder) => this.m_memberPath.ToCompactString(builder);

    internal string ToUserString() => this.m_memberPath.PathToString(new bool?(false));

    protected override bool IsEqualTo(ProjectedSlot right) => right is MemberProjectedSlot memberProjectedSlot && MemberPath.EqualityComparer.Equals(this.m_memberPath, memberProjectedSlot.m_memberPath);

    protected override int GetHash() => MemberPath.EqualityComparer.GetHashCode(this.m_memberPath);

    internal MemberProjectedSlot RemapSlot(
      Dictionary<MemberPath, MemberPath> remap)
    {
      MemberPath node = (MemberPath) null;
      return remap.TryGetValue(this.MemberPath, out node) ? new MemberProjectedSlot(node) : new MemberProjectedSlot(this.MemberPath);
    }

    internal static List<MemberProjectedSlot> GetKeySlots(
      IEnumerable<MemberProjectedSlot> slots,
      MemberPath prefix)
    {
      EntitySet entitySet = prefix.EntitySet;
      List<ExtentKey> keysForEntityType = ExtentKey.GetKeysForEntityType(prefix, entitySet.ElementType);
      return MemberProjectedSlot.GetSlots(slots, keysForEntityType[0].KeyFields);
    }

    internal static List<MemberProjectedSlot> GetSlots(
      IEnumerable<MemberProjectedSlot> slots,
      IEnumerable<MemberPath> members)
    {
      List<MemberProjectedSlot> memberProjectedSlotList = new List<MemberProjectedSlot>();
      foreach (MemberPath member in members)
      {
        MemberProjectedSlot slotForMember = MemberProjectedSlot.GetSlotForMember(Helpers.AsSuperTypeList<MemberProjectedSlot, ProjectedSlot>(slots), member);
        if (slotForMember == null)
          return (List<MemberProjectedSlot>) null;
        memberProjectedSlotList.Add(slotForMember);
      }
      return memberProjectedSlotList;
    }

    internal static MemberProjectedSlot GetSlotForMember(
      IEnumerable<ProjectedSlot> slots,
      MemberPath member)
    {
      foreach (MemberProjectedSlot slot in slots)
      {
        if (MemberPath.EqualityComparer.Equals(slot.MemberPath, member))
          return slot;
      }
      return (MemberProjectedSlot) null;
    }
  }
}
