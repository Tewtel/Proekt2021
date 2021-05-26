// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.TypeInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class TypeInfo
  {
    private readonly TypeUsage m_type;
    private readonly List<TypeInfo> m_immediateSubTypes;
    private readonly TypeInfo m_superType;
    private readonly RootTypeInfo m_rootType;

    internal static TypeInfo Create(
      TypeUsage type,
      TypeInfo superTypeInfo,
      ExplicitDiscriminatorMap discriminatorMap)
    {
      return superTypeInfo != null ? new TypeInfo(type, superTypeInfo) : (TypeInfo) new RootTypeInfo(type, discriminatorMap);
    }

    protected TypeInfo(TypeUsage type, TypeInfo superType)
    {
      this.m_type = type;
      this.m_immediateSubTypes = new List<TypeInfo>();
      this.m_superType = superType;
      if (superType == null)
        return;
      superType.m_immediateSubTypes.Add(this);
      this.m_rootType = superType.RootType;
    }

    internal bool IsRootType => this.m_rootType == null;

    internal List<TypeInfo> ImmediateSubTypes => this.m_immediateSubTypes;

    internal TypeInfo SuperType => this.m_superType;

    internal RootTypeInfo RootType => this.m_rootType ?? (RootTypeInfo) this;

    internal TypeUsage Type => this.m_type;

    internal object TypeId { get; set; }

    internal virtual RowType FlattenedType => this.RootType.FlattenedType;

    internal virtual TypeUsage FlattenedTypeUsage => this.RootType.FlattenedTypeUsage;

    internal virtual EdmProperty EntitySetIdProperty => this.RootType.EntitySetIdProperty;

    internal bool HasEntitySetIdProperty => this.RootType.EntitySetIdProperty != null;

    internal virtual EdmProperty NullSentinelProperty => this.RootType.NullSentinelProperty;

    internal bool HasNullSentinelProperty => this.RootType.NullSentinelProperty != null;

    internal virtual EdmProperty TypeIdProperty => this.RootType.TypeIdProperty;

    internal bool HasTypeIdProperty => this.RootType.TypeIdProperty != null;

    internal virtual IEnumerable<PropertyRef> PropertyRefList => this.RootType.PropertyRefList;

    internal EdmProperty GetNewProperty(PropertyRef propertyRef)
    {
      EdmProperty newProperty;
      this.TryGetNewProperty(propertyRef, true, out newProperty);
      return newProperty;
    }

    internal bool TryGetNewProperty(
      PropertyRef propertyRef,
      bool throwIfMissing,
      out EdmProperty newProperty)
    {
      return this.RootType.TryGetNewProperty(propertyRef, throwIfMissing, out newProperty);
    }

    internal IEnumerable<PropertyRef> GetKeyPropertyRefs()
    {
      RefType type = (RefType) null;
      foreach (EdmMember keyMember in (!TypeHelpers.TryGetEdmType<RefType>(this.m_type, out type) ? TypeHelpers.GetEdmType<EntityTypeBase>(this.m_type) : type.ElementType).KeyMembers)
      {
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(keyMember is EdmProperty, "Non-EdmProperty key members are not supported");
        yield return (PropertyRef) new SimplePropertyRef(keyMember);
      }
    }

    internal IEnumerable<PropertyRef> GetIdentityPropertyRefs()
    {
      if (this.HasEntitySetIdProperty)
        yield return (PropertyRef) EntitySetIdPropertyRef.Instance;
      foreach (PropertyRef keyPropertyRef in this.GetKeyPropertyRefs())
        yield return keyPropertyRef;
    }

    internal IEnumerable<PropertyRef> GetAllPropertyRefs()
    {
      foreach (PropertyRef propertyRef in this.PropertyRefList)
        yield return propertyRef;
    }

    internal IEnumerable<EdmProperty> GetAllProperties()
    {
      foreach (EdmProperty property in this.FlattenedType.Properties)
        yield return property;
    }

    internal List<TypeInfo> GetTypeHierarchy()
    {
      List<TypeInfo> result = new List<TypeInfo>();
      this.GetTypeHierarchy(result);
      return result;
    }

    private void GetTypeHierarchy(List<TypeInfo> result)
    {
      result.Add(this);
      foreach (TypeInfo immediateSubType in this.ImmediateSubTypes)
        immediateSubType.GetTypeHierarchy(result);
    }
  }
}
