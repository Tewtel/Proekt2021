// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.NotMappedTypeAttributeConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to process instances of <see cref="T:System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute" /> found on types in the model.
  /// </summary>
  public class NotMappedTypeAttributeConvention : 
    TypeAttributeConfigurationConvention<NotMappedAttribute>
  {
    /// <inheritdoc />
    public override void Apply(
      ConventionTypeConfiguration configuration,
      NotMappedAttribute attribute)
    {
      System.Data.Entity.Utilities.Check.NotNull<ConventionTypeConfiguration>(configuration, nameof (configuration));
      System.Data.Entity.Utilities.Check.NotNull<NotMappedAttribute>(attribute, nameof (attribute));
      configuration.Ignore();
    }
  }
}
