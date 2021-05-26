// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation.ForeignKeyConstraintConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation
{
  internal class ForeignKeyConstraintConfiguration : ConstraintConfiguration
  {
    private readonly List<PropertyInfo> _dependentProperties = new List<PropertyInfo>();
    private readonly bool _isFullySpecified;

    public ForeignKeyConstraintConfiguration()
    {
    }

    internal ForeignKeyConstraintConfiguration(IEnumerable<PropertyInfo> dependentProperties)
    {
      this._dependentProperties.AddRange(dependentProperties);
      this._isFullySpecified = true;
    }

    private ForeignKeyConstraintConfiguration(ForeignKeyConstraintConfiguration source)
    {
      this._dependentProperties.AddRange((IEnumerable<PropertyInfo>) source._dependentProperties);
      this._isFullySpecified = source._isFullySpecified;
    }

    internal override ConstraintConfiguration Clone() => (ConstraintConfiguration) new ForeignKeyConstraintConfiguration(this);

    public override bool IsFullySpecified => this._isFullySpecified;

    internal IEnumerable<PropertyInfo> ToProperties => (IEnumerable<PropertyInfo>) this._dependentProperties;

    public void AddColumn(PropertyInfo propertyInfo)
    {
      System.Data.Entity.Utilities.Check.NotNull<PropertyInfo>(propertyInfo, nameof (propertyInfo));
      if (this._dependentProperties.ContainsSame(propertyInfo))
        return;
      this._dependentProperties.Add(propertyInfo);
    }

    internal override void Configure(
      AssociationType associationType,
      AssociationEndMember dependentEnd,
      EntityTypeConfiguration entityTypeConfiguration)
    {
      if (!this._dependentProperties.Any<PropertyInfo>())
        return;
      IEnumerable<PropertyInfo> propertyInfos = this._dependentProperties.AsEnumerable<PropertyInfo>();
      if (!this.IsFullySpecified)
      {
        if (EntityTypeExtensions.GetClrType(dependentEnd.GetEntityType()) != entityTypeConfiguration.ClrType)
          return;
        IEnumerable<\u003C\u003Ef__AnonymousType54<PropertyInfo, int?>> source = this._dependentProperties.Select(p => new
        {
          PropertyInfo = p,
          ColumnOrder = entityTypeConfiguration.Property(new PropertyPath(p)).ColumnOrder
        });
        if (this._dependentProperties.Count > 1 && source.Any(p => !p.ColumnOrder.HasValue))
        {
          ReadOnlyMetadataCollection<EdmProperty> dependentKeys = dependentEnd.GetEntityType().KeyProperties;
          if (dependentKeys.Count != this._dependentProperties.Count || !source.All(fk => dependentKeys.Any<EdmProperty>((Func<EdmProperty, bool>) (p => p.GetClrPropertyInfo().IsSameAs(fk.PropertyInfo)))))
            throw System.Data.Entity.Resources.Error.ForeignKeyAttributeConvention_OrderRequired((object) entityTypeConfiguration.ClrType);
          propertyInfos = dependentKeys.Select<EdmProperty, PropertyInfo>((Func<EdmProperty, PropertyInfo>) (p => p.GetClrPropertyInfo()));
        }
        else
          propertyInfos = source.OrderBy(p => p.ColumnOrder).Select(p => p.PropertyInfo);
      }
      List<EdmProperty> edmPropertyList = new List<EdmProperty>();
      foreach (PropertyInfo propertyInfo in propertyInfos)
        edmPropertyList.Add(dependentEnd.GetEntityType().GetDeclaredPrimitiveProperty(propertyInfo) ?? throw System.Data.Entity.Resources.Error.ForeignKeyPropertyNotFound((object) propertyInfo.Name, (object) dependentEnd.GetEntityType().Name));
      AssociationEndMember otherEnd = associationType.GetOtherEnd(dependentEnd);
      ReferentialConstraint referentialConstraint = new ReferentialConstraint((RelationshipEndMember) otherEnd, (RelationshipEndMember) dependentEnd, (IEnumerable<EdmProperty>) otherEnd.GetEntityType().KeyProperties, (IEnumerable<EdmProperty>) edmPropertyList);
      if (otherEnd.IsRequired())
        referentialConstraint.ToProperties.Each<EdmProperty, bool>((Func<EdmProperty, bool>) (p => p.Nullable = false));
      associationType.Constraint = referentialConstraint;
    }

    public bool Equals(ForeignKeyConstraintConfiguration other)
    {
      if (other == null)
        return false;
      return this == other || other.ToProperties.SequenceEqual<PropertyInfo>(this.ToProperties, (IEqualityComparer<PropertyInfo>) new DynamicEqualityComparer<PropertyInfo>((Func<PropertyInfo, PropertyInfo, bool>) ((p1, p2) => p1.IsSameAs(p2))));
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != typeof (ForeignKeyConstraintConfiguration)) && this.Equals((ForeignKeyConstraintConfiguration) obj);
    }

    public override int GetHashCode() => this.ToProperties.Aggregate<PropertyInfo, int>(0, (Func<int, PropertyInfo, int>) ((t, p) => t + p.GetHashCode()));
  }
}
