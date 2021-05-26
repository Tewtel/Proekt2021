// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.TypeNameForeignKeyDiscoveryConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to discover foreign key properties whose names are a combination
  /// of the principal type name and the principal type primary key property name(s).
  /// </summary>
  public class TypeNameForeignKeyDiscoveryConvention : ForeignKeyDiscoveryConvention
  {
    /// <inheritdoc />
    protected override bool MatchDependentKeyProperty(
      AssociationType associationType,
      AssociationEndMember dependentAssociationEnd,
      EdmProperty dependentProperty,
      EntityType principalEntityType,
      EdmProperty principalKeyProperty)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationType>(associationType, nameof (associationType));
      System.Data.Entity.Utilities.Check.NotNull<AssociationEndMember>(dependentAssociationEnd, nameof (dependentAssociationEnd));
      System.Data.Entity.Utilities.Check.NotNull<EdmProperty>(dependentProperty, nameof (dependentProperty));
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(principalEntityType, nameof (principalEntityType));
      System.Data.Entity.Utilities.Check.NotNull<EdmProperty>(principalKeyProperty, nameof (principalKeyProperty));
      return string.Equals(dependentProperty.Name, principalEntityType.Name + principalKeyProperty.Name, StringComparison.OrdinalIgnoreCase);
    }
  }
}
