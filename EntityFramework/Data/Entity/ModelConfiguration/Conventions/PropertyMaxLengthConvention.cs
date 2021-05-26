﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.PropertyMaxLengthConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to set a maximum length for properties whose type supports length facets. The default value is 128.
  /// </summary>
  public class PropertyMaxLengthConvention : 
    IConceptualModelConvention<EntityType>,
    IConvention,
    IConceptualModelConvention<ComplexType>,
    IConceptualModelConvention<AssociationType>
  {
    private const int DefaultLength = 128;
    private readonly int _length;

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.ModelConfiguration.Conventions.PropertyMaxLengthConvention" /> with the default length.
    /// </summary>
    public PropertyMaxLengthConvention()
      : this(128)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.ModelConfiguration.Conventions.PropertyMaxLengthConvention" /> with the specified length.
    /// </summary>
    /// <param name="length">The maximum length of properties.</param>
    public PropertyMaxLengthConvention(int length) => this._length = length > 0 ? length : throw new ArgumentOutOfRangeException(nameof (length), System.Data.Entity.Resources.Strings.InvalidMaxLengthSize);

    /// <inheritdoc />
    public virtual void Apply(EntityType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      this.SetLength((IEnumerable<EdmProperty>) item.DeclaredProperties, (ICollection<EdmProperty>) item.KeyProperties);
    }

    /// <inheritdoc />
    public virtual void Apply(ComplexType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<ComplexType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      this.SetLength((IEnumerable<EdmProperty>) item.Properties, (ICollection<EdmProperty>) new List<EdmProperty>());
    }

    private void SetLength(
      IEnumerable<EdmProperty> properties,
      ICollection<EdmProperty> keyProperties)
    {
      foreach (EdmProperty property in properties)
      {
        if (property.IsPrimitiveType)
        {
          if (property.PrimitiveType == PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String))
            this.SetStringDefaults(property, keyProperties.Contains(property));
          if (property.PrimitiveType == PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Binary))
            this.SetBinaryDefaults(property, keyProperties.Contains(property));
        }
      }
    }

    /// <inheritdoc />
    public virtual void Apply(AssociationType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      if (item.Constraint == null)
        return;
      IEnumerable<EdmProperty> source = item.GetOtherEnd(item.Constraint.DependentEnd).GetEntityType().KeyProperties();
      if (source.Count<EdmProperty>() != item.Constraint.ToProperties.Count)
        return;
      for (int index = 0; index < item.Constraint.ToProperties.Count; ++index)
      {
        EdmProperty toProperty = item.Constraint.ToProperties[index];
        EdmProperty edmProperty = source.ElementAt<EdmProperty>(index);
        if (toProperty.PrimitiveType == PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String) || toProperty.PrimitiveType == PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Binary))
        {
          toProperty.IsUnicode = edmProperty.IsUnicode;
          toProperty.IsFixedLength = edmProperty.IsFixedLength;
          toProperty.MaxLength = edmProperty.MaxLength;
          toProperty.IsMaxLength = edmProperty.IsMaxLength;
        }
      }
    }

    private void SetStringDefaults(EdmProperty property, bool isKey)
    {
      if (!property.IsUnicode.HasValue)
        property.IsUnicode = new bool?(true);
      this.SetBinaryDefaults(property, isKey);
    }

    private void SetBinaryDefaults(EdmProperty property, bool isKey)
    {
      if (!property.IsFixedLength.HasValue)
        property.IsFixedLength = new bool?(false);
      if (property.MaxLength.HasValue || property.IsMaxLength)
        return;
      if (!isKey)
      {
        bool? isFixedLength = property.IsFixedLength;
        bool flag = true;
        if (!(isFixedLength.GetValueOrDefault() == flag & isFixedLength.HasValue))
        {
          property.IsMaxLength = true;
          return;
        }
      }
      property.MaxLength = new int?(this._length);
    }
  }
}
