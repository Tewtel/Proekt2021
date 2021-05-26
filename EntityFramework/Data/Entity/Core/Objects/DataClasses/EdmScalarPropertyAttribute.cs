// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.EdmScalarPropertyAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>
  /// Attribute for scalar properties in an IEntity.
  /// Implied default AttributeUsage properties Inherited=True, AllowMultiple=False,
  /// The metadata system expects this and will only look at the first of each of these attributes, even if there are more.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property)]
  public sealed class EdmScalarPropertyAttribute : EdmPropertyAttribute
  {
    private bool _isNullable = true;

    /// <summary>Gets or sets the value that indicates whether the property can have a null value.</summary>
    /// <returns>The value that indicates whether the property can have a null value.</returns>
    public bool IsNullable
    {
      get => this._isNullable;
      set => this._isNullable = value;
    }

    /// <summary>Gets or sets the value that indicates whether the property is part of the entity key.</summary>
    /// <returns>The value that indicates whether the property is part of the entity key.</returns>
    public bool EntityKeyProperty { get; set; }
  }
}
