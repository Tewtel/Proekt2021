// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ColumnMappingBuilder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  internal class ColumnMappingBuilder
  {
    private EdmProperty _columnProperty;
    private readonly IList<EdmProperty> _propertyPath;
    private ScalarPropertyMapping _scalarPropertyMapping;

    public ColumnMappingBuilder(EdmProperty columnProperty, IList<EdmProperty> propertyPath)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmProperty>(columnProperty, nameof (columnProperty));
      System.Data.Entity.Utilities.Check.NotNull<IList<EdmProperty>>(propertyPath, nameof (propertyPath));
      this._columnProperty = columnProperty;
      this._propertyPath = propertyPath;
    }

    public IList<EdmProperty> PropertyPath => this._propertyPath;

    public EdmProperty ColumnProperty
    {
      get => this._columnProperty;
      internal set
      {
        this._columnProperty = value;
        if (this._scalarPropertyMapping == null)
          return;
        this._scalarPropertyMapping.Column = this._columnProperty;
      }
    }

    internal void SetTarget(ScalarPropertyMapping scalarPropertyMapping) => this._scalarPropertyMapping = scalarPropertyMapping;
  }
}
