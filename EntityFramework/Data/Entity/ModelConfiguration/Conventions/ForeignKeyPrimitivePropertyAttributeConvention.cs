// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.ForeignKeyPrimitivePropertyAttributeConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;
using System.Data.Entity.ModelConfiguration.Mappers;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to process instances of <see cref="T:System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute" /> found on foreign key properties in the model.
  /// </summary>
  public class ForeignKeyPrimitivePropertyAttributeConvention : 
    PropertyAttributeConfigurationConvention<ForeignKeyAttribute>
  {
    /// <inheritdoc />
    public override void Apply(
      PropertyInfo memberInfo,
      ConventionTypeConfiguration configuration,
      ForeignKeyAttribute attribute)
    {
      System.Data.Entity.Utilities.Check.NotNull<PropertyInfo>(memberInfo, nameof (memberInfo));
      System.Data.Entity.Utilities.Check.NotNull<ConventionTypeConfiguration>(configuration, nameof (configuration));
      System.Data.Entity.Utilities.Check.NotNull<ForeignKeyAttribute>(attribute, nameof (attribute));
      if (!memberInfo.IsValidEdmScalarProperty())
        return;
      PropertyInfo propertyInfo = new PropertyFilter().GetProperties(configuration.ClrType, false).Where<PropertyInfo>((Func<PropertyInfo, bool>) (pi => pi.Name.Equals(attribute.Name, StringComparison.Ordinal))).SingleOrDefault<PropertyInfo>();
      if (propertyInfo == (PropertyInfo) null)
        throw System.Data.Entity.Resources.Error.ForeignKeyAttributeConvention_InvalidNavigationProperty((object) memberInfo.Name, (object) configuration.ClrType, (object) attribute.Name);
      configuration.NavigationProperty(propertyInfo).HasConstraint<ForeignKeyConstraintConfiguration>((Action<ForeignKeyConstraintConfiguration>) (fk => fk.AddColumn(memberInfo)));
    }
  }
}
