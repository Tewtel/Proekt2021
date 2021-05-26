// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.InversePropertyAttributeConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Mappers;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to process instances of <see cref="T:System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute" /> found on properties in the model.
  /// </summary>
  public class InversePropertyAttributeConvention : 
    PropertyAttributeConfigurationConvention<InversePropertyAttribute>
  {
    /// <inheritdoc />
    public override void Apply(
      PropertyInfo memberInfo,
      ConventionTypeConfiguration configuration,
      InversePropertyAttribute attribute)
    {
      System.Data.Entity.Utilities.Check.NotNull<PropertyInfo>(memberInfo, nameof (memberInfo));
      System.Data.Entity.Utilities.Check.NotNull<ConventionTypeConfiguration>(configuration, nameof (configuration));
      System.Data.Entity.Utilities.Check.NotNull<InversePropertyAttribute>(attribute, nameof (attribute));
      if (!memberInfo.IsValidEdmNavigationProperty())
        return;
      Type targetType = memberInfo.PropertyType.GetTargetType();
      PropertyInfo inverseNavigationProperty = new PropertyFilter().GetProperties(targetType, false).SingleOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => string.Equals(p.Name, attribute.Property, StringComparison.OrdinalIgnoreCase)));
      if (inverseNavigationProperty == (PropertyInfo) null)
        throw System.Data.Entity.Resources.Error.InversePropertyAttributeConvention_PropertyNotFound((object) attribute.Property, (object) targetType, (object) memberInfo.Name, (object) memberInfo.ReflectedType);
      if (memberInfo == inverseNavigationProperty)
        throw System.Data.Entity.Resources.Error.InversePropertyAttributeConvention_SelfInverseDetected((object) memberInfo.Name, (object) memberInfo.ReflectedType);
      configuration.NavigationProperty(memberInfo).HasInverseNavigationProperty((Func<PropertyInfo, PropertyInfo>) (p => inverseNavigationProperty));
    }
  }
}
