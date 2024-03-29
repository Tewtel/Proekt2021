﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.MaterializedDataRecord
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;
using System.Globalization;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class MaterializedDataRecord : 
    DbDataRecord,
    IExtendedDataRecord,
    IDataRecord,
    ICustomTypeDescriptor
  {
    private FieldNameLookup _fieldNameLookup;
    private DataRecordInfo _recordInfo;
    private readonly MetadataWorkspace _workspace;
    private readonly TypeUsage _edmUsage;
    private readonly object[] _values;
    private PropertyDescriptorCollection _propertyDescriptors;
    private MaterializedDataRecord.FilterCache _filterCache;
    private Dictionary<object, AttributeCollection> _attrCache;

    internal MaterializedDataRecord(
      MetadataWorkspace workspace,
      TypeUsage edmUsage,
      object[] values)
    {
      this._workspace = workspace;
      this._edmUsage = edmUsage;
      this._values = values;
    }

    public DataRecordInfo DataRecordInfo
    {
      get
      {
        if (this._recordInfo == null)
          this._recordInfo = this._workspace != null ? new DataRecordInfo(this._workspace.GetOSpaceTypeUsage(this._edmUsage)) : new DataRecordInfo(this._edmUsage);
        return this._recordInfo;
      }
    }

    public override int FieldCount => this._values.Length;

    public override object this[int ordinal] => this.GetValue(ordinal);

    public override object this[string name] => this.GetValue(this.GetOrdinal(name));

    public override bool GetBoolean(int ordinal) => (bool) this._values[ordinal];

    public override byte GetByte(int ordinal) => (byte) this._values[ordinal];

    public override long GetBytes(
      int ordinal,
      long fieldOffset,
      byte[] buffer,
      int bufferOffset,
      int length)
    {
      byte[] numArray = (byte[]) this._values[ordinal];
      int length1 = numArray.Length;
      int sourceIndex = fieldOffset <= (long) int.MaxValue ? (int) fieldOffset : throw new ArgumentOutOfRangeException(nameof (fieldOffset), Strings.ADP_InvalidSourceBufferIndex((object) length1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) fieldOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (buffer == null)
        return (long) length1;
      try
      {
        if (sourceIndex < length1)
        {
          if (sourceIndex + length > length1)
            length1 -= sourceIndex;
          else
            length1 = length;
        }
        Array.Copy((Array) numArray, sourceIndex, (Array) buffer, bufferOffset, length1);
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
        {
          int length2 = numArray.Length;
          if (length < 0)
            throw new IndexOutOfRangeException(Strings.ADP_InvalidDataLength((object) ((long) length).ToString((IFormatProvider) CultureInfo.InvariantCulture)));
          if (bufferOffset < 0 || bufferOffset >= buffer.Length)
            throw new ArgumentOutOfRangeException(nameof (bufferOffset), Strings.ADP_InvalidDestinationBufferIndex((object) length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) bufferOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
          if (fieldOffset < 0L || fieldOffset >= (long) length2)
            throw new ArgumentOutOfRangeException(nameof (fieldOffset), Strings.ADP_InvalidSourceBufferIndex((object) length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) fieldOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
          if (length2 + bufferOffset > buffer.Length)
            throw new IndexOutOfRangeException(Strings.ADP_InvalidBufferSizeOrIndex((object) length2.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) bufferOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
        }
        throw;
      }
      return (long) length1;
    }

    public override char GetChar(int ordinal) => ((string) this.GetValue(ordinal))[0];

    public override long GetChars(
      int ordinal,
      long fieldOffset,
      char[] buffer,
      int bufferOffset,
      int length)
    {
      string str = (string) this._values[ordinal];
      int count = str.Length;
      int sourceIndex = fieldOffset <= (long) int.MaxValue ? (int) fieldOffset : throw new ArgumentOutOfRangeException(nameof (fieldOffset), Strings.ADP_InvalidSourceBufferIndex((object) count.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) fieldOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (buffer == null)
        return (long) count;
      try
      {
        if (sourceIndex < count)
        {
          if (sourceIndex + length > count)
            count -= sourceIndex;
          else
            count = length;
        }
        str.CopyTo(sourceIndex, buffer, bufferOffset, count);
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
        {
          int length1 = str.Length;
          if (length < 0)
            throw new IndexOutOfRangeException(Strings.ADP_InvalidDataLength((object) ((long) length).ToString((IFormatProvider) CultureInfo.InvariantCulture)));
          if (bufferOffset < 0 || bufferOffset >= buffer.Length)
            throw new ArgumentOutOfRangeException(nameof (bufferOffset), Strings.ADP_InvalidDestinationBufferIndex((object) buffer.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) bufferOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
          if (fieldOffset < 0L || fieldOffset >= (long) length1)
            throw new ArgumentOutOfRangeException(nameof (fieldOffset), Strings.ADP_InvalidSourceBufferIndex((object) length1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) fieldOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
          if (length1 + bufferOffset > buffer.Length)
            throw new IndexOutOfRangeException(Strings.ADP_InvalidBufferSizeOrIndex((object) length1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) bufferOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
        }
        throw;
      }
      return (long) count;
    }

    public DbDataRecord GetDataRecord(int ordinal) => (DbDataRecord) this._values[ordinal];

    public DbDataReader GetDataReader(int i) => this.GetDbDataReader(i);

    public override string GetDataTypeName(int ordinal) => this.GetMember(ordinal).TypeUsage.EdmType.Name;

    public override DateTime GetDateTime(int ordinal) => (DateTime) this._values[ordinal];

    public override Decimal GetDecimal(int ordinal) => (Decimal) this._values[ordinal];

    public override double GetDouble(int ordinal) => (double) this._values[ordinal];

    public override Type GetFieldType(int ordinal)
    {
      Type clrType = this.GetMember(ordinal).TypeUsage.EdmType.ClrType;
      return (object) clrType != null ? clrType : typeof (object);
    }

    public override float GetFloat(int ordinal) => (float) this._values[ordinal];

    public override Guid GetGuid(int ordinal) => (Guid) this._values[ordinal];

    public override short GetInt16(int ordinal) => (short) this._values[ordinal];

    public override int GetInt32(int ordinal) => (int) this._values[ordinal];

    public override long GetInt64(int ordinal) => (long) this._values[ordinal];

    public override string GetName(int ordinal) => this.GetMember(ordinal).Name;

    public override int GetOrdinal(string name)
    {
      if (this._fieldNameLookup == null)
        this._fieldNameLookup = new FieldNameLookup((IDataRecord) this);
      return this._fieldNameLookup.GetOrdinal(name);
    }

    public override string GetString(int ordinal) => (string) this._values[ordinal];

    public override object GetValue(int ordinal) => this._values[ordinal];

    public override int GetValues(object[] values)
    {
      System.Data.Entity.Utilities.Check.NotNull<object[]>(values, nameof (values));
      int num = Math.Min(values.Length, this.FieldCount);
      for (int index = 0; index < num; ++index)
        values[index] = this._values[index];
      return num;
    }

    private EdmMember GetMember(int ordinal) => this.DataRecordInfo.FieldMetadata[ordinal].FieldType;

    public override bool IsDBNull(int ordinal) => DBNull.Value == this._values[ordinal];

    AttributeCollection ICustomTypeDescriptor.GetAttributes() => TypeDescriptor.GetAttributes((object) this, true);

    string ICustomTypeDescriptor.GetClassName() => (string) null;

    string ICustomTypeDescriptor.GetComponentName() => (string) null;

    private PropertyDescriptorCollection InitializePropertyDescriptors()
    {
      if (this._values == null)
        return (PropertyDescriptorCollection) null;
      if (this._propertyDescriptors == null && this._values.Length != 0)
        this._propertyDescriptors = MaterializedDataRecord.CreatePropertyDescriptorCollection(this.DataRecordInfo.RecordType.EdmType as StructuralType, typeof (MaterializedDataRecord), true);
      return this._propertyDescriptors;
    }

    internal static PropertyDescriptorCollection CreatePropertyDescriptorCollection(
      StructuralType structuralType,
      Type componentType,
      bool isReadOnly)
    {
      List<PropertyDescriptor> propertyDescriptorList = new List<PropertyDescriptor>();
      if (structuralType != null)
      {
        foreach (EdmMember member in structuralType.Members)
        {
          if (member.BuiltInTypeKind == BuiltInTypeKind.EdmProperty)
          {
            EdmProperty property = (EdmProperty) member;
            FieldDescriptor fieldDescriptor = new FieldDescriptor(componentType, isReadOnly, property);
            propertyDescriptorList.Add((PropertyDescriptor) fieldDescriptor);
          }
        }
      }
      return new PropertyDescriptorCollection(propertyDescriptorList.ToArray());
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => ((ICustomTypeDescriptor) this).GetProperties((Attribute[]) null);

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(
      Attribute[] attributes)
    {
      bool flag = attributes != null && (uint) attributes.Length > 0U;
      PropertyDescriptorCollection descriptorCollection1 = this.InitializePropertyDescriptors();
      if (descriptorCollection1 == null)
        return descriptorCollection1;
      MaterializedDataRecord.FilterCache filterCache = this._filterCache;
      if (flag && filterCache != null && filterCache.IsValid(attributes))
        return filterCache.FilteredProperties;
      if (!flag && descriptorCollection1 != null)
        return descriptorCollection1;
      if (this._attrCache == null && attributes != null && attributes.Length != 0)
      {
        this._attrCache = new Dictionary<object, AttributeCollection>();
        foreach (FieldDescriptor propertyDescriptor in this._propertyDescriptors)
        {
          object[] customAttributes = propertyDescriptor.GetValue((object) this).GetType().GetCustomAttributes(false);
          Attribute[] attributeArray = new Attribute[customAttributes.Length];
          customAttributes.CopyTo((Array) attributeArray, 0);
          this._attrCache.Add((object) propertyDescriptor, new AttributeCollection(attributeArray));
        }
      }
      PropertyDescriptorCollection descriptorCollection2 = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
      foreach (PropertyDescriptor propertyDescriptor in this._propertyDescriptors)
      {
        if (this._attrCache[(object) propertyDescriptor].Matches(attributes))
          descriptorCollection2.Add(propertyDescriptor);
      }
      if (flag)
        this._filterCache = new MaterializedDataRecord.FilterCache()
        {
          Attributes = attributes,
          FilteredProperties = descriptorCollection2
        };
      return descriptorCollection2;
    }

    object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) => (object) this;

    private class FilterCache
    {
      public Attribute[] Attributes;
      public PropertyDescriptorCollection FilteredProperties;

      public bool IsValid(Attribute[] other)
      {
        if (other == null || this.Attributes == null || this.Attributes.Length != other.Length)
          return false;
        for (int index = 0; index < other.Length; ++index)
        {
          if (!this.Attributes[index].Match((object) other[index]))
            return false;
        }
        return true;
      }
    }
  }
}
