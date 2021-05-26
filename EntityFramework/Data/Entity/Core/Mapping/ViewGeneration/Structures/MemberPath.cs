// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.MemberPath
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal sealed class MemberPath : InternalBase, IEquatable<MemberPath>
  {
    private readonly EntitySetBase m_extent;
    private readonly List<EdmMember> m_path;
    internal static readonly IEqualityComparer<MemberPath> EqualityComparer = (IEqualityComparer<MemberPath>) new MemberPath.Comparer();

    internal MemberPath(EntitySetBase extent, IEnumerable<EdmMember> path)
    {
      this.m_extent = extent;
      this.m_path = path.ToList<EdmMember>();
    }

    internal MemberPath(EntitySetBase extent)
      : this(extent, Enumerable.Empty<EdmMember>())
    {
    }

    internal MemberPath(EntitySetBase extent, EdmMember member)
      : this(extent, Enumerable.Repeat<EdmMember>(member, 1))
    {
    }

    internal MemberPath(MemberPath prefix, EdmMember last)
    {
      this.m_extent = prefix.m_extent;
      this.m_path = new List<EdmMember>((IEnumerable<EdmMember>) prefix.m_path);
      this.m_path.Add(last);
    }

    internal EdmMember RootEdmMember => this.m_path.Count <= 0 ? (EdmMember) null : this.m_path[0];

    internal EdmMember LeafEdmMember => this.m_path.Count <= 0 ? (EdmMember) null : this.m_path[this.m_path.Count - 1];

    internal string LeafName => this.m_path.Count == 0 ? this.m_extent.Name : this.LeafEdmMember.Name;

    internal bool IsComputed => this.m_path.Count != 0 && this.RootEdmMember.IsStoreGeneratedComputed;

    internal object DefaultValue
    {
      get
      {
        if (this.m_path.Count == 0)
          return (object) null;
        Facet facet;
        return this.LeafEdmMember.TypeUsage.Facets.TryGetValue(nameof (DefaultValue), false, out facet) ? facet.Value : (object) null;
      }
    }

    internal bool IsPartOfKey => this.m_path.Count != 0 && MetadataHelper.IsPartOfEntityTypeKey(this.LeafEdmMember);

    internal bool IsNullable => this.m_path.Count != 0 && MetadataHelper.IsMemberNullable(this.LeafEdmMember);

    internal EntitySet EntitySet
    {
      get
      {
        if (this.m_path.Count == 0)
          return this.m_extent as EntitySet;
        return this.m_path.Count == 1 && this.RootEdmMember is AssociationEndMember rootEdmMember ? MetadataHelper.GetEntitySetAtEnd((AssociationSet) this.m_extent, rootEdmMember) : (EntitySet) null;
      }
    }

    internal EntitySetBase Extent => this.m_extent;

    internal EdmType EdmType => this.m_path.Count > 0 ? this.LeafEdmMember.TypeUsage.EdmType : (EdmType) this.m_extent.ElementType;

    internal string CqlFieldAlias
    {
      get
      {
        string name = this.PathToString(new bool?(true));
        if (!name.Contains("_"))
          name = name.Replace('.', '_');
        StringBuilder builder = new StringBuilder();
        CqlWriter.AppendEscapedName(builder, name);
        return builder.ToString();
      }
    }

    internal bool IsAlwaysDefined(
      Dictionary<EntityType, Set<EntityType>> inheritanceGraph)
    {
      if (this.m_path.Count == 0)
        return true;
      EdmMember member = this.m_path.Last<EdmMember>();
      for (int index = 0; index < this.m_path.Count - 1; ++index)
      {
        if (MetadataHelper.IsMemberNullable(this.m_path[index]))
          return false;
      }
      if (this.m_path[0].DeclaringType is AssociationType || !(this.m_extent.ElementType is EntityType elementType))
        return true;
      EntityType declaringType = this.m_path[0].DeclaringType as EntityType;
      EntityType baseType = declaringType.BaseType as EntityType;
      if (elementType.EdmEquals((MetadataItem) declaringType) || MetadataHelper.IsParentOf(declaringType, elementType) || baseType == null)
        return true;
      return (baseType.Abstract || MetadataHelper.DoesMemberExist((StructuralType) baseType, member)) && !MemberPath.RecurseToFindMemberAbsentInConcreteType(baseType, declaringType, member, elementType, inheritanceGraph);
    }

    private static bool RecurseToFindMemberAbsentInConcreteType(
      EntityType current,
      EntityType avoidEdge,
      EdmMember member,
      EntityType entitySetType,
      Dictionary<EntityType, Set<EntityType>> inheritanceGraph)
    {
      foreach (EntityType current1 in inheritanceGraph[current].Where<EntityType>((Func<EntityType, bool>) (type => !type.EdmEquals((MetadataItem) avoidEdge))))
      {
        if ((entitySetType.BaseType == null || !entitySetType.BaseType.EdmEquals((MetadataItem) current1)) && (!current1.Abstract && !MetadataHelper.DoesMemberExist((StructuralType) current1, member) || MemberPath.RecurseToFindMemberAbsentInConcreteType(current1, current, member, entitySetType, inheritanceGraph)))
          return true;
      }
      return false;
    }

    internal void GetIdentifiers(CqlIdentifiers identifiers)
    {
      identifiers.AddIdentifier(this.m_extent.Name);
      identifiers.AddIdentifier(this.m_extent.ElementType.Name);
      foreach (EdmMember edmMember in this.m_path)
        identifiers.AddIdentifier(edmMember.Name);
    }

    internal static bool AreAllMembersNullable(IEnumerable<MemberPath> members)
    {
      foreach (MemberPath member in members)
      {
        if (member.m_path.Count == 0 || !member.IsNullable)
          return false;
      }
      return true;
    }

    internal static string PropertiesToUserString(IEnumerable<MemberPath> members, bool fullPath)
    {
      bool flag = true;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (MemberPath member in members)
      {
        if (!flag)
          stringBuilder.Append(", ");
        flag = false;
        if (fullPath)
          stringBuilder.Append(member.PathToString(new bool?(false)));
        else
          stringBuilder.Append(member.LeafName);
      }
      return stringBuilder.ToString();
    }

    internal StringBuilder AsEsql(StringBuilder inputBuilder, string blockAlias)
    {
      StringBuilder builder = new StringBuilder();
      CqlWriter.AppendEscapedName(builder, blockAlias);
      this.AsCql((Action<string>) (memberName =>
      {
        builder.Append('.');
        CqlWriter.AppendEscapedName(builder, memberName);
      }), (Action) (() =>
      {
        builder.Insert(0, "Key(");
        builder.Append(")");
      }), (Action<StructuralType>) (treatAsType =>
      {
        builder.Insert(0, "TREAT(");
        builder.Append(" AS ");
        CqlWriter.AppendEscapedTypeName(builder, (EdmType) treatAsType);
        builder.Append(')');
      }));
      inputBuilder.Append((object) builder);
      return inputBuilder;
    }

    internal DbExpression AsCqt(DbExpression row)
    {
      this.AsCql((Action<string>) (memberName => row = (DbExpression) row.Property(memberName)), (Action) (() => row = (DbExpression) row.GetRefKey()), (Action<StructuralType>) (treatAsType => row = (DbExpression) row.TreatAs(TypeUsage.Create((EdmType) treatAsType))));
      return row;
    }

    internal void AsCql(Action<string> accessMember, Action getKey, Action<StructuralType> treatAs)
    {
      EdmType edmType = (EdmType) this.m_extent.ElementType;
      foreach (EdmMember member in this.m_path)
      {
        RefType refType;
        StructuralType type;
        if (Helper.IsRefType((GlobalItem) edmType))
        {
          refType = (RefType) edmType;
          type = (StructuralType) refType.ElementType;
        }
        else
        {
          refType = (RefType) null;
          type = (StructuralType) edmType;
        }
        bool flag = MetadataHelper.DoesMemberExist(type, member);
        if (refType != null)
          getKey();
        else if (!flag)
          treatAs(member.DeclaringType);
        accessMember(member.Name);
        edmType = member.TypeUsage.EdmType;
      }
    }

    public bool Equals(MemberPath right) => MemberPath.EqualityComparer.Equals(this, right);

    public override bool Equals(object obj)
    {
      MemberPath right = obj as MemberPath;
      return obj != null && this.Equals(right);
    }

    public override int GetHashCode() => MemberPath.EqualityComparer.GetHashCode(this);

    internal bool IsScalarType() => this.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType || this.EdmType.BuiltInTypeKind == BuiltInTypeKind.EnumType;

    internal static IEnumerable<MemberPath> GetKeyMembers(
      EntitySetBase extent,
      MemberDomainMap domainMap)
    {
      MemberPath memberPath = new MemberPath(extent);
      return (IEnumerable<MemberPath>) new List<MemberPath>(memberPath.GetMembers((EdmType) memberPath.Extent.ElementType, new bool?(), new bool?(), new bool?(true), domainMap));
    }

    internal IEnumerable<MemberPath> GetMembers(
      EdmType edmType,
      bool? isScalar,
      bool? isConditional,
      bool? isPartOfKey,
      MemberDomainMap domainMap)
    {
      MemberPath currentPath = this;
      foreach (EdmMember member1 in ((StructuralType) edmType).Members)
      {
        EdmMember edmMember = member1;
        if (edmMember is AssociationEndMember)
        {
          foreach (MemberPath member2 in new MemberPath(currentPath, edmMember).GetMembers((EdmType) ((RefType) edmMember.TypeUsage.EdmType).ElementType, isScalar, isConditional, new bool?(true), domainMap))
            yield return member2;
        }
        bool flag1 = MetadataHelper.IsNonRefSimpleMember(edmMember);
        bool? nullable;
        if (isScalar.HasValue)
        {
          nullable = isScalar;
          bool flag2 = flag1;
          if (!(nullable.GetValueOrDefault() == flag2 & nullable.HasValue))
            goto label_13;
        }
        if (edmMember is EdmProperty edmProperty2)
        {
          bool flag2 = MetadataHelper.IsPartOfEntityTypeKey((EdmMember) edmProperty2);
          if (isPartOfKey.HasValue)
          {
            nullable = isPartOfKey;
            bool flag3 = flag2;
            if (!(nullable.GetValueOrDefault() == flag3 & nullable.HasValue))
              goto label_13;
          }
          MemberPath path = new MemberPath(currentPath, (EdmMember) edmProperty2);
          bool flag4 = domainMap.IsConditionMember(path);
          if (isConditional.HasValue)
          {
            nullable = isConditional;
            bool flag3 = flag4;
            if (!(nullable.GetValueOrDefault() == flag3 & nullable.HasValue))
              goto label_13;
          }
          yield return path;
        }
label_13:
        edmMember = (EdmMember) null;
      }
    }

    internal bool IsEquivalentViaRefConstraint(MemberPath path1)
    {
      MemberPath assocPath0_1 = this;
      if (assocPath0_1.EdmType is EntityTypeBase || path1.EdmType is EntityTypeBase || (!MetadataHelper.IsNonRefSimpleMember(assocPath0_1.LeafEdmMember) || !MetadataHelper.IsNonRefSimpleMember(path1.LeafEdmMember)))
        return false;
      AssociationSet extent1 = assocPath0_1.Extent as AssociationSet;
      AssociationSet extent2 = path1.Extent as AssociationSet;
      EntitySet extent3 = assocPath0_1.Extent as EntitySet;
      EntitySet extent4 = path1.Extent as EntitySet;
      bool flag = false;
      if (extent1 != null && extent2 != null)
      {
        if (!extent1.Equals((object) extent2))
          return false;
        flag = MemberPath.AreAssociationEndPathsEquivalentViaRefConstraint(assocPath0_1, path1, extent1);
      }
      else if (extent3 != null && extent4 != null)
      {
        foreach (AssociationSet associationsForEntitySet in MetadataHelper.GetAssociationsForEntitySets(extent3, extent4))
        {
          if (MemberPath.AreAssociationEndPathsEquivalentViaRefConstraint(assocPath0_1.GetCorrespondingAssociationPath(associationsForEntitySet), path1.GetCorrespondingAssociationPath(associationsForEntitySet), associationsForEntitySet))
          {
            flag = true;
            break;
          }
        }
      }
      else
      {
        AssociationSet assocSet = extent1 ?? extent2;
        MemberPath assocPath0_2 = assocPath0_1.Extent is AssociationSet ? assocPath0_1 : path1;
        MemberPath correspondingAssociationPath = (assocPath0_1.Extent is EntitySet ? assocPath0_1 : path1).GetCorrespondingAssociationPath(assocSet);
        flag = correspondingAssociationPath != null && MemberPath.AreAssociationEndPathsEquivalentViaRefConstraint(assocPath0_2, correspondingAssociationPath, assocSet);
      }
      return flag;
    }

    private static bool AreAssociationEndPathsEquivalentViaRefConstraint(
      MemberPath assocPath0,
      MemberPath assocPath1,
      AssociationSet assocSet)
    {
      AssociationEndMember rootEdmMember1 = assocPath0.RootEdmMember as AssociationEndMember;
      AssociationEndMember rootEdmMember2 = assocPath1.RootEdmMember as AssociationEndMember;
      EdmProperty leafEdmMember1 = assocPath0.LeafEdmMember as EdmProperty;
      EdmProperty leafEdmMember2 = assocPath1.LeafEdmMember as EdmProperty;
      if (rootEdmMember1 == null || rootEdmMember2 == null || (leafEdmMember1 == null || leafEdmMember2 == null))
        return false;
      AssociationType elementType = assocSet.ElementType;
      bool flag1 = false;
      foreach (ReferentialConstraint referentialConstraint in elementType.ReferentialConstraints)
      {
        bool flag2 = rootEdmMember1.Name == referentialConstraint.FromRole.Name && rootEdmMember2.Name == referentialConstraint.ToRole.Name;
        bool flag3 = rootEdmMember2.Name == referentialConstraint.FromRole.Name && rootEdmMember1.Name == referentialConstraint.ToRole.Name;
        if (flag2 | flag3)
        {
          ReadOnlyMetadataCollection<EdmProperty> metadataCollection1 = flag2 ? referentialConstraint.FromProperties : referentialConstraint.ToProperties;
          ReadOnlyMetadataCollection<EdmProperty> metadataCollection2 = flag2 ? referentialConstraint.ToProperties : referentialConstraint.FromProperties;
          int num1 = metadataCollection1.IndexOf(leafEdmMember1);
          EdmProperty edmProperty = leafEdmMember2;
          int num2 = metadataCollection2.IndexOf(edmProperty);
          if (num1 == num2 && num1 != -1)
          {
            flag1 = true;
            break;
          }
        }
      }
      return flag1;
    }

    private MemberPath GetCorrespondingAssociationPath(AssociationSet assocSet)
    {
      AssociationEndMember someEndForEntitySet = MetadataHelper.GetSomeEndForEntitySet(assocSet, this.m_extent);
      if (someEndForEntitySet == null)
        return (MemberPath) null;
      List<EdmMember> edmMemberList = new List<EdmMember>();
      edmMemberList.Add((EdmMember) someEndForEntitySet);
      edmMemberList.AddRange((IEnumerable<EdmMember>) this.m_path);
      return new MemberPath((EntitySetBase) assocSet, (IEnumerable<EdmMember>) edmMemberList);
    }

    internal EntitySet GetScopeOfRelationEnd()
    {
      if (this.m_path.Count == 0)
        return (EntitySet) null;
      return !(this.LeafEdmMember is AssociationEndMember leafEdmMember) ? (EntitySet) null : MetadataHelper.GetEntitySetAtEnd((AssociationSet) this.m_extent, leafEdmMember);
    }

    internal string PathToString(bool? forAlias)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (forAlias.HasValue)
      {
        bool? nullable = forAlias;
        bool flag = true;
        if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        {
          if (this.m_path.Count == 0)
            return this.m_extent.ElementType.Name;
          stringBuilder.Append(this.m_path[0].DeclaringType.Name);
        }
        else
          stringBuilder.Append(this.m_extent.Name);
      }
      for (int index = 0; index < this.m_path.Count; ++index)
      {
        stringBuilder.Append('.');
        stringBuilder.Append(this.m_path[index].Name);
      }
      return stringBuilder.ToString();
    }

    internal override void ToCompactString(StringBuilder builder) => builder.Append(this.PathToString(new bool?(false)));

    internal void ToCompactString(StringBuilder builder, string instanceToken) => builder.Append(instanceToken + this.PathToString(new bool?()));

    private sealed class Comparer : IEqualityComparer<MemberPath>
    {
      public bool Equals(MemberPath left, MemberPath right)
      {
        if (left == right)
          return true;
        if (left == null || right == null || (!left.m_extent.Equals((object) right.m_extent) || left.m_path.Count != right.m_path.Count))
          return false;
        for (int index = 0; index < left.m_path.Count; ++index)
        {
          if (!left.m_path[index].Equals((object) right.m_path[index]))
            return false;
        }
        return true;
      }

      public int GetHashCode(MemberPath key)
      {
        int hashCode = key.m_extent.GetHashCode();
        foreach (EdmMember edmMember in key.m_path)
          hashCode ^= edmMember.GetHashCode();
        return hashCode;
      }
    }
  }
}
