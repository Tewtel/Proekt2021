// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.RootTypeInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class RootTypeInfo : TypeInfo
  {
    private readonly List<PropertyRef> m_propertyRefList;
    private readonly Dictionary<PropertyRef, EdmProperty> m_propertyMap;
    private EdmProperty m_nullSentinelProperty;
    private EdmProperty m_typeIdProperty;
    private readonly ExplicitDiscriminatorMap m_discriminatorMap;
    private EdmProperty m_entitySetIdProperty;
    private RowType m_flattenedType;
    private TypeUsage m_flattenedTypeUsage;

    internal RootTypeInfo(TypeUsage type, ExplicitDiscriminatorMap discriminatorMap)
      : base(type, (TypeInfo) null)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(type.EdmType.BaseType == null, "only root types allowed here");
      this.m_propertyMap = new Dictionary<PropertyRef, EdmProperty>();
      this.m_propertyRefList = new List<PropertyRef>();
      this.m_discriminatorMap = discriminatorMap;
      this.TypeIdKind = TypeIdKind.Generated;
    }

    internal TypeIdKind TypeIdKind { get; set; }

    internal TypeUsage TypeIdType { get; set; }

    internal void AddPropertyMapping(PropertyRef propertyRef, EdmProperty newProperty)
    {
      this.m_propertyMap[propertyRef] = newProperty;
      switch (propertyRef)
      {
        case TypeIdPropertyRef _:
          this.m_typeIdProperty = newProperty;
          break;
        case EntitySetIdPropertyRef _:
          this.m_entitySetIdProperty = newProperty;
          break;
        case NullSentinelPropertyRef _:
          this.m_nullSentinelProperty = newProperty;
          break;
      }
    }

    internal void AddPropertyRef(PropertyRef propertyRef) => this.m_propertyRefList.Add(propertyRef);

    internal new RowType FlattenedType
    {
      get => this.m_flattenedType;
      set
      {
        this.m_flattenedType = value;
        this.m_flattenedTypeUsage = TypeUsage.Create((EdmType) value);
      }
    }

    internal new TypeUsage FlattenedTypeUsage => this.m_flattenedTypeUsage;

    internal ExplicitDiscriminatorMap DiscriminatorMap => this.m_discriminatorMap;

    internal new EdmProperty EntitySetIdProperty => this.m_entitySetIdProperty;

    internal new EdmProperty NullSentinelProperty => this.m_nullSentinelProperty;

    internal new IEnumerable<PropertyRef> PropertyRefList => (IEnumerable<PropertyRef>) this.m_propertyRefList;

    internal int GetNestedStructureOffset(PropertyRef property)
    {
      for (int index = 0; index < this.m_propertyRefList.Count; ++index)
      {
        if (this.m_propertyRefList[index] is NestedPropertyRef propertyRef1 && propertyRef1.InnerProperty.Equals((object) property))
          return index;
      }
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(false, "no complex structure " + property?.ToString() + " found in TypeInfo");
      return 0;
    }

    internal new bool TryGetNewProperty(
      PropertyRef propertyRef,
      bool throwIfMissing,
      out EdmProperty property)
    {
      bool flag = this.m_propertyMap.TryGetValue(propertyRef, out property);
      if (throwIfMissing && !flag)
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(false, "Unable to find property " + propertyRef?.ToString() + " in type " + this.Type.EdmType.Identity);
      return flag;
    }

    internal new EdmProperty TypeIdProperty => this.m_typeIdProperty;
  }
}
