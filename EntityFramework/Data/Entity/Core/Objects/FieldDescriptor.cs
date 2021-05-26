// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.FieldDescriptor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class FieldDescriptor : PropertyDescriptor
  {
    private readonly EdmProperty _property;
    private readonly Type _fieldType;
    private readonly Type _itemType;
    private readonly bool _isReadOnly;

    internal FieldDescriptor(string propertyName)
      : base(propertyName, (Attribute[]) null)
    {
    }

    internal FieldDescriptor(Type itemType, bool isReadOnly, EdmProperty property)
      : base(property.Name, (Attribute[]) null)
    {
      this._itemType = itemType;
      this._property = property;
      this._isReadOnly = isReadOnly;
      this._fieldType = this.DetermineClrType(this._property.TypeUsage);
    }

    private Type DetermineClrType(TypeUsage typeUsage)
    {
      Type type = (Type) null;
      EdmType edmType = typeUsage.EdmType;
      switch (edmType.BuiltInTypeKind)
      {
        case BuiltInTypeKind.CollectionType:
          type = typeof (IEnumerable<>).MakeGenericType(this.DetermineClrType(((CollectionType) edmType).TypeUsage));
          break;
        case BuiltInTypeKind.ComplexType:
        case BuiltInTypeKind.EntityType:
          type = edmType.ClrType;
          break;
        case BuiltInTypeKind.EnumType:
        case BuiltInTypeKind.PrimitiveType:
          type = edmType.ClrType;
          Facet facet;
          if (type.IsValueType() && typeUsage.Facets.TryGetValue("Nullable", false, out facet) && (bool) facet.Value)
          {
            type = typeof (Nullable<>).MakeGenericType(type);
            break;
          }
          break;
        case BuiltInTypeKind.RefType:
          type = typeof (EntityKey);
          break;
        case BuiltInTypeKind.RowType:
          type = typeof (IDataRecord);
          break;
      }
      return type;
    }

    internal EdmProperty EdmProperty => this._property;

    public override Type ComponentType => this._itemType;

    public override bool IsReadOnly => this._isReadOnly;

    public override Type PropertyType => this._fieldType;

    public override bool CanResetValue(object item) => false;

    public override object GetValue(object item)
    {
      System.Data.Entity.Utilities.Check.NotNull<object>(item, nameof (item));
      if (!this._itemType.IsAssignableFrom(item.GetType()))
        throw new ArgumentException(Strings.ObjectView_IncompatibleArgument);
      return !(item is DbDataRecord dbDataRecord) ? DelegateFactory.GetValue(this._property, item) : dbDataRecord.GetValue(dbDataRecord.GetOrdinal(this._property.Name));
    }

    public override void ResetValue(object item) => throw new NotSupportedException();

    public override void SetValue(object item, object value)
    {
      System.Data.Entity.Utilities.Check.NotNull<object>(item, nameof (item));
      if (!this._itemType.IsAssignableFrom(item.GetType()))
        throw new ArgumentException(Strings.ObjectView_IncompatibleArgument);
      if (this._isReadOnly)
        throw new InvalidOperationException(Strings.ObjectView_WriteOperationNotAllowedOnReadOnlyBindingList);
      DelegateFactory.SetValue(this._property, item, value);
    }

    public override bool ShouldSerializeValue(object item) => false;

    public override bool IsBrowsable => true;
  }
}
