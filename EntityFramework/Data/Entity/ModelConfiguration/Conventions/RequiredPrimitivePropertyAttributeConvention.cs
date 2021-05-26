// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.RequiredPrimitivePropertyAttributeConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to process instances of <see cref="T:System.ComponentModel.DataAnnotations.RequiredAttribute" /> found on primitive properties in the model.
  /// </summary>
  public class RequiredPrimitivePropertyAttributeConvention : 
    PrimitivePropertyAttributeConfigurationConvention<RequiredAttribute>
  {
    /// <inheritdoc />
    public override void Apply(
      ConventionPrimitivePropertyConfiguration configuration,
      RequiredAttribute attribute)
    {
      System.Data.Entity.Utilities.Check.NotNull<ConventionPrimitivePropertyConfiguration>(configuration, nameof (configuration));
      System.Data.Entity.Utilities.Check.NotNull<RequiredAttribute>(attribute, nameof (attribute));
      configuration.IsRequired();
    }
  }
}
