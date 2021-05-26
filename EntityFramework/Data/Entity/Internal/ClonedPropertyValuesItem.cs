// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ClonedPropertyValuesItem
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Internal
{
  internal class ClonedPropertyValuesItem : IPropertyValuesItem
  {
    private readonly string _name;
    private readonly bool _isComplex;
    private readonly Type _type;

    public ClonedPropertyValuesItem(string name, object value, Type type, bool isComplex)
    {
      this._name = name;
      this._type = type;
      this._isComplex = isComplex;
      this.Value = value;
    }

    public object Value { get; set; }

    public string Name => this._name;

    public bool IsComplex => this._isComplex;

    public Type Type => this._type;
  }
}
