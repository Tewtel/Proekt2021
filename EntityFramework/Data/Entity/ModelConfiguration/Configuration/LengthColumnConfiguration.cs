// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.LengthColumnConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Used to configure a column with length facets for an entity type or complex type. This configuration functionality is exposed by the Code First Fluent API, see <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// </summary>
  public abstract class LengthColumnConfiguration : PrimitiveColumnConfiguration
  {
    internal LengthColumnConfiguration(System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.LengthPropertyConfiguration configuration)
      : base((System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration) configuration)
    {
    }

    internal System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.LengthPropertyConfiguration Configuration => (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.LengthPropertyConfiguration) base.Configuration;

    /// <summary>Configures the column to allow the maximum length supported by the database provider.</summary>
    /// <returns>The same <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.LengthColumnConfiguration" /> instance so that multiple calls can be chained.</returns>
    public LengthColumnConfiguration IsMaxLength()
    {
      this.Configuration.IsMaxLength = new bool?(true);
      this.Configuration.MaxLength = new int?();
      return this;
    }

    /// <summary>Configures the column to have the specified maximum length.</summary>
    /// <returns>The same <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.LengthColumnConfiguration" /> instance so that multiple calls can be chained.</returns>
    /// <param name="value">The maximum length for the column. Setting the value to null will remove any maximum length restriction from the column and a default length will be used for the database column.</param>
    public LengthColumnConfiguration HasMaxLength(int? value)
    {
      this.Configuration.MaxLength = value;
      this.Configuration.IsMaxLength = new bool?();
      return this;
    }

    /// <summary>Configures the column to be fixed length.</summary>
    /// <returns>The same <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.LengthColumnConfiguration" /> instance so that multiple calls can be chained.</returns>
    public LengthColumnConfiguration IsFixedLength()
    {
      this.Configuration.IsFixedLength = new bool?(true);
      return this;
    }

    /// <summary>Configures the column to be variable length.</summary>
    /// <returns>The same <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.LengthColumnConfiguration" /> instance so that multiple calls can be chained.</returns>
    public LengthColumnConfiguration IsVariableLength()
    {
      this.Configuration.IsFixedLength = new bool?(false);
      return this;
    }

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
