// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataRecordObjectView
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class DataRecordObjectView : ObjectView<DbDataRecord>, ITypedList
  {
    private readonly PropertyDescriptorCollection _propertyDescriptorsCache;
    private readonly RowType _rowType;

    internal DataRecordObjectView(
      IObjectViewData<DbDataRecord> viewData,
      object eventDataSource,
      RowType rowType,
      Type propertyComponentType)
      : base(viewData, eventDataSource)
    {
      if (!typeof (IDataRecord).IsAssignableFrom(propertyComponentType))
        propertyComponentType = typeof (IDataRecord);
      this._rowType = rowType;
      this._propertyDescriptorsCache = MaterializedDataRecord.CreatePropertyDescriptorCollection((StructuralType) this._rowType, propertyComponentType, true);
    }

    private static PropertyInfo GetTypedIndexer(Type type)
    {
      PropertyInfo propertyInfo1 = (PropertyInfo) null;
      if (typeof (IList).IsAssignableFrom(type) || typeof (ITypedList).IsAssignableFrom(type) || typeof (IListSource).IsAssignableFrom(type))
      {
        foreach (PropertyInfo propertyInfo2 in type.GetInstanceProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.IsPublic())))
        {
          if (propertyInfo2.GetIndexParameters().Length != 0 && propertyInfo2.PropertyType != typeof (object))
          {
            propertyInfo1 = propertyInfo2;
            if (propertyInfo1.Name == "Item")
              break;
          }
        }
      }
      return propertyInfo1;
    }

    private static Type GetListItemType(Type type)
    {
      Type type1;
      if (typeof (Array).IsAssignableFrom(type))
      {
        type1 = type.GetElementType();
      }
      else
      {
        PropertyInfo typedIndexer = DataRecordObjectView.GetTypedIndexer(type);
        type1 = !(typedIndexer != (PropertyInfo) null) ? type : typedIndexer.PropertyType;
      }
      return type1;
    }

    PropertyDescriptorCollection ITypedList.GetItemProperties(
      PropertyDescriptor[] listAccessors)
    {
      PropertyDescriptorCollection descriptorCollection;
      if (listAccessors == null || listAccessors.Length == 0)
      {
        descriptorCollection = this._propertyDescriptorsCache;
      }
      else
      {
        PropertyDescriptor listAccessor = listAccessors[listAccessors.Length - 1];
        descriptorCollection = !(listAccessor is FieldDescriptor fieldDescriptor2) || fieldDescriptor2.EdmProperty == null || fieldDescriptor2.EdmProperty.TypeUsage.EdmType.BuiltInTypeKind != BuiltInTypeKind.RowType ? TypeDescriptor.GetProperties(DataRecordObjectView.GetListItemType(listAccessor.PropertyType)) : MaterializedDataRecord.CreatePropertyDescriptorCollection((StructuralType) fieldDescriptor2.EdmProperty.TypeUsage.EdmType, typeof (IDataRecord), true);
      }
      return descriptorCollection;
    }

    string ITypedList.GetListName(PropertyDescriptor[] listAccessors) => this._rowType.Name;
  }
}
