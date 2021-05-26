// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.FacetDescription
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;
using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Class for representing a FacetDescription object</summary>
  public class FacetDescription
  {
    private readonly string _facetName;
    private readonly EdmType _facetType;
    private readonly int? _minValue;
    private readonly int? _maxValue;
    private readonly object _defaultValue;
    private readonly bool _isConstant;
    private Facet _defaultValueFacet;
    private Facet _nullValueFacet;
    private Facet[] _valueCache;
    private static readonly object _notInitializedSentinel = new object();

    internal FacetDescription()
    {
    }

    internal FacetDescription(
      string facetName,
      EdmType facetType,
      int? minValue,
      int? maxValue,
      object defaultValue,
      bool isConstant,
      string declaringTypeName)
    {
      this._facetName = facetName;
      this._facetType = facetType;
      this._minValue = minValue;
      this._maxValue = maxValue;
      this._defaultValue = defaultValue == null ? FacetDescription._notInitializedSentinel : defaultValue;
      this._isConstant = isConstant;
      this.Validate(declaringTypeName);
      if (!this._isConstant)
        return;
      FacetDescription.UpdateMinMaxValueForConstant(this._facetName, this._facetType, ref this._minValue, ref this._maxValue, this._defaultValue);
    }

    internal FacetDescription(
      string facetName,
      EdmType facetType,
      int? minValue,
      int? maxValue,
      object defaultValue)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(facetName, nameof (facetName));
      System.Data.Entity.Utilities.Check.NotNull<EdmType>(facetType, nameof (facetType));
      if ((minValue.HasValue || maxValue.HasValue) && minValue.HasValue)
      {
        int num = maxValue.HasValue ? 1 : 0;
      }
      this._facetName = facetName;
      this._facetType = facetType;
      this._minValue = minValue;
      this._maxValue = maxValue;
      this._defaultValue = defaultValue;
    }

    /// <summary>Gets the name of this facet.</summary>
    /// <returns>The name of this facet.</returns>
    public virtual string FacetName => this._facetName;

    /// <summary>Gets the type of this facet.</summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmType" /> object that represents the type of this facet.
    /// </returns>
    public EdmType FacetType => this._facetType;

    /// <summary>Gets the minimum value for this facet.</summary>
    /// <returns>The minimum value for this facet.</returns>
    public int? MinValue => this._minValue;

    /// <summary>Gets the maximum value for this facet.</summary>
    /// <returns>The maximum value for this facet.</returns>
    public int? MaxValue => this._maxValue;

    /// <summary>Gets the default value of a facet with this facet description.</summary>
    /// <returns>The default value of a facet with this facet description.</returns>
    public object DefaultValue => this._defaultValue == FacetDescription._notInitializedSentinel ? (object) null : this._defaultValue;

    /// <summary>Gets a value indicating whether the value of this facet is a constant.</summary>
    /// <returns>true if this facet is a constant; otherwise, false. </returns>
    public virtual bool IsConstant => this._isConstant;

    /// <summary>Gets a value indicating whether this facet is a required facet.</summary>
    /// <returns>true if this facet is a required facet; otherwise, false.</returns>
    public bool IsRequired => this._defaultValue == FacetDescription._notInitializedSentinel;

    internal Facet DefaultValueFacet
    {
      get
      {
        if (this._defaultValueFacet == null)
          Interlocked.CompareExchange<Facet>(ref this._defaultValueFacet, Facet.Create(this, this.DefaultValue, true), (Facet) null);
        return this._defaultValueFacet;
      }
    }

    internal Facet NullValueFacet
    {
      get
      {
        if (this._nullValueFacet == null)
          Interlocked.CompareExchange<Facet>(ref this._nullValueFacet, Facet.Create(this, (object) null, true), (Facet) null);
        return this._nullValueFacet;
      }
    }

    /// <summary>Returns the name of this facet. </summary>
    /// <returns>The name of this facet.</returns>
    public override string ToString() => this.FacetName;

    internal Facet GetBooleanFacet(bool value)
    {
      if (this._valueCache == null)
        Interlocked.CompareExchange<Facet[]>(ref this._valueCache, new Facet[2]
        {
          Facet.Create(this, (object) true, true),
          Facet.Create(this, (object) false, true)
        }, (Facet[]) null);
      return !value ? this._valueCache[1] : this._valueCache[0];
    }

    internal static bool IsNumericType(EdmType facetType)
    {
      if (!Helper.IsPrimitiveType(facetType))
        return false;
      PrimitiveType primitiveType = (PrimitiveType) facetType;
      return primitiveType.PrimitiveTypeKind == PrimitiveTypeKind.Byte || primitiveType.PrimitiveTypeKind == PrimitiveTypeKind.SByte || primitiveType.PrimitiveTypeKind == PrimitiveTypeKind.Int16 || primitiveType.PrimitiveTypeKind == PrimitiveTypeKind.Int32;
    }

    private static void UpdateMinMaxValueForConstant(
      string facetName,
      EdmType facetType,
      ref int? minValue,
      ref int? maxValue,
      object defaultValue)
    {
      if (!FacetDescription.IsNumericType(facetType))
        return;
      if (facetName == "Precision" || facetName == "Scale")
      {
        ref int? local1 = ref minValue;
        byte? nullable1 = (byte?) defaultValue;
        int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        local1 = nullable2;
        ref int? local2 = ref maxValue;
        byte? nullable3 = (byte?) defaultValue;
        int? nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        local2 = nullable4;
      }
      else
      {
        minValue = (int?) defaultValue;
        maxValue = (int?) defaultValue;
      }
    }

    private void Validate(string declaringTypeName)
    {
      if (this._defaultValue == FacetDescription._notInitializedSentinel)
      {
        if (this._isConstant)
          throw new ArgumentException(Strings.MissingDefaultValueForConstantFacet((object) this._facetName, (object) declaringTypeName));
      }
      else
      {
        if (!FacetDescription.IsNumericType(this._facetType))
          return;
        if (this._isConstant)
        {
          if (this._minValue.HasValue != this._maxValue.HasValue || this._minValue.HasValue && this._minValue.Value != this._maxValue.Value)
            throw new ArgumentException(Strings.MinAndMaxValueMustBeSameForConstantFacet((object) this._facetName, (object) declaringTypeName));
        }
        else
        {
          if (!this._minValue.HasValue || !this._maxValue.HasValue)
            throw new ArgumentException(Strings.BothMinAndMaxValueMustBeSpecifiedForNonConstantFacet((object) this._facetName, (object) declaringTypeName));
          int num1 = this._minValue.Value;
          int? nullable = this._maxValue;
          int valueOrDefault = nullable.GetValueOrDefault();
          if (num1 == valueOrDefault & nullable.HasValue)
            throw new ArgumentException(Strings.MinAndMaxValueMustBeDifferentForNonConstantFacet((object) this._facetName, (object) declaringTypeName));
          nullable = this._minValue;
          int num2 = 0;
          if (!(nullable.GetValueOrDefault() < num2 & nullable.HasValue))
          {
            nullable = this._maxValue;
            int num3 = 0;
            if (!(nullable.GetValueOrDefault() < num3 & nullable.HasValue))
            {
              nullable = this._minValue;
              int? maxValue = this._maxValue;
              if (!(nullable.GetValueOrDefault() > maxValue.GetValueOrDefault() & (nullable.HasValue & maxValue.HasValue)))
                return;
              throw new ArgumentException(Strings.MinMustBeLessThanMax((object) this._minValue.ToString(), (object) this._facetName, (object) declaringTypeName));
            }
          }
          throw new ArgumentException(Strings.MinAndMaxMustBePositive((object) this._facetName, (object) declaringTypeName));
        }
      }
    }
  }
}
