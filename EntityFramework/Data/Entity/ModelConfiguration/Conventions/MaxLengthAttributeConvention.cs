﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.MaxLengthAttributeConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Resources;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to process instances of <see cref="T:System.ComponentModel.DataAnnotations.MaxLengthAttribute" /> found on properties in the model.
  /// </summary>
  public class MaxLengthAttributeConvention : 
    PrimitivePropertyAttributeConfigurationConvention<MaxLengthAttribute>
  {
    private const int MaxLengthIndicator = -1;

    /// <inheritdoc />
    public override void Apply(
      ConventionPrimitivePropertyConfiguration configuration,
      MaxLengthAttribute attribute)
    {
      System.Data.Entity.Utilities.Check.NotNull<ConventionPrimitivePropertyConfiguration>(configuration, nameof (configuration));
      System.Data.Entity.Utilities.Check.NotNull<MaxLengthAttribute>(attribute, nameof (attribute));
      PropertyInfo clrPropertyInfo = configuration.ClrPropertyInfo;
      if (attribute.Length == 0 || attribute.Length < -1)
        throw Error.MaxLengthAttributeConvention_InvalidMaxLength((object) clrPropertyInfo.Name, (object) clrPropertyInfo.ReflectedType);
      if (attribute.Length == -1)
        configuration.IsMaxLength();
      else
        configuration.HasMaxLength(attribute.Length);
    }
  }
}
