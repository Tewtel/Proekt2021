// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.StateManagerMemberMetadata
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Objects
{
  internal class StateManagerMemberMetadata
  {
    private readonly EdmProperty _clrProperty;
    private readonly EdmProperty _edmProperty;
    private readonly bool _isPartOfKey;
    private readonly bool _isComplexType;

    internal StateManagerMemberMetadata()
    {
    }

    internal StateManagerMemberMetadata(
      ObjectPropertyMapping memberMap,
      EdmProperty memberMetadata,
      bool isPartOfKey)
    {
      this._clrProperty = memberMap.ClrProperty;
      this._edmProperty = memberMetadata;
      this._isPartOfKey = isPartOfKey;
      this._isComplexType = Helper.IsEntityType(this._edmProperty.TypeUsage.EdmType) || Helper.IsComplexType(this._edmProperty.TypeUsage.EdmType);
    }

    internal string CLayerName => this._edmProperty.Name;

    internal Type ClrType => this._clrProperty.TypeUsage.EdmType.ClrType;

    internal virtual bool IsComplex => this._isComplexType;

    internal virtual EdmProperty CdmMetadata => this._edmProperty;

    internal EdmProperty ClrMetadata => this._clrProperty;

    internal bool IsPartOfKey => this._isPartOfKey;

    public virtual object GetValue(object userObject) => DelegateFactory.GetValue(this._clrProperty, userObject);

    public void SetValue(object userObject, object value)
    {
      if (DBNull.Value == value)
        value = (object) null;
      if (this.IsComplex && value == null)
        throw new InvalidOperationException(Strings.ComplexObject_NullableComplexTypesNotSupported((object) this.CLayerName));
      DelegateFactory.SetValue(this._clrProperty, userObject, value);
    }
  }
}
