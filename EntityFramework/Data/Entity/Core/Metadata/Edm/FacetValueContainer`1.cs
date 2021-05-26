// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.FacetValueContainer`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal struct FacetValueContainer<T>
  {
    private T _value;
    private bool _hasValue;
    private bool _isUnbounded;

    internal T Value
    {
      set
      {
        this._isUnbounded = false;
        this._hasValue = true;
        this._value = value;
      }
    }

    private void SetUnbounded()
    {
      this._isUnbounded = true;
      this._hasValue = true;
    }

    public static implicit operator FacetValueContainer<T>(
      EdmConstants.Unbounded unbounded)
    {
      FacetValueContainer<T> facetValueContainer = new FacetValueContainer<T>();
      facetValueContainer.SetUnbounded();
      return facetValueContainer;
    }

    public static implicit operator FacetValueContainer<T>(T value) => new FacetValueContainer<T>()
    {
      Value = value
    };

    internal object GetValueAsObject() => this._isUnbounded ? (object) EdmConstants.UnboundedValue : (object) this._value;

    internal bool HasValue => this._hasValue;
  }
}
