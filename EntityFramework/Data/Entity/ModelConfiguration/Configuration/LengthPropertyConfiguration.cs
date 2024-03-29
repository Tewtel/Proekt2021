﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.LengthPropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Used to configure a property with length facets for an entity type or complex type.
  /// This configuration functionality is available via the Code First Fluent API, see <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// </summary>
  public abstract class LengthPropertyConfiguration : PrimitivePropertyConfiguration
  {
    internal LengthPropertyConfiguration(System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.LengthPropertyConfiguration configuration)
      : base((System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration) configuration)
    {
    }

    /// <summary>
    /// Configures the property to allow the maximum length supported by the database provider.
    /// </summary>
    /// <returns> The same LengthPropertyConfiguration instance so that multiple calls can be chained. </returns>
    public LengthPropertyConfiguration IsMaxLength()
    {
      this.Configuration.IsMaxLength = new bool?(true);
      this.Configuration.MaxLength = new int?();
      return this;
    }

    /// <summary>
    /// Configures the property to have the specified maximum length.
    /// </summary>
    /// <param name="value"> The maximum length for the property. Setting 'null' will remove any maximum length restriction from the property and a default length will be used for the database column. </param>
    /// <returns> The same LengthPropertyConfiguration instance so that multiple calls can be chained. </returns>
    public LengthPropertyConfiguration HasMaxLength(int? value)
    {
      this.Configuration.MaxLength = value;
      this.Configuration.IsMaxLength = new bool?();
      this.Configuration.IsFixedLength = new bool?(this.Configuration.IsFixedLength.GetValueOrDefault());
      return this;
    }

    /// <summary>
    /// Configures the property to be fixed length.
    /// Use HasMaxLength to set the length that the property is fixed to.
    /// </summary>
    /// <returns> The same LengthPropertyConfiguration instance so that multiple calls can be chained. </returns>
    public LengthPropertyConfiguration IsFixedLength()
    {
      this.Configuration.IsFixedLength = new bool?(true);
      return this;
    }

    /// <summary>
    /// Configures the property to be variable length.
    /// Properties are variable length by default.
    /// </summary>
    /// <returns> The same LengthPropertyConfiguration instance so that multiple calls can be chained. </returns>
    public LengthPropertyConfiguration IsVariableLength()
    {
      this.Configuration.IsFixedLength = new bool?(false);
      return this;
    }

    internal System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.LengthPropertyConfiguration Configuration => (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.LengthPropertyConfiguration) base.Configuration;
  }
}
