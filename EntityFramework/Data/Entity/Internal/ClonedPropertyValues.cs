// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ClonedPropertyValues
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;

namespace System.Data.Entity.Internal
{
  internal class ClonedPropertyValues : InternalPropertyValues
  {
    private readonly ISet<string> _propertyNames;
    private readonly IDictionary<string, ClonedPropertyValuesItem> _propertyValues;

    internal ClonedPropertyValues(InternalPropertyValues original, DbDataRecord valuesRecord = null)
      : base(original.InternalContext, original.ObjectType, original.IsEntityValues)
    {
      this._propertyNames = original.PropertyNames;
      this._propertyValues = (IDictionary<string, ClonedPropertyValuesItem>) new Dictionary<string, ClonedPropertyValuesItem>(this._propertyNames.Count);
      foreach (string propertyName in (IEnumerable<string>) this._propertyNames)
      {
        IPropertyValuesItem propertyValuesItem = original.GetItem(propertyName);
        object obj = propertyValuesItem.Value;
        if (obj is InternalPropertyValues original3)
        {
          DbDataRecord valuesRecord1 = valuesRecord == null ? (DbDataRecord) null : (DbDataRecord) valuesRecord[propertyName];
          obj = (object) new ClonedPropertyValues(original3, valuesRecord1);
        }
        else if (valuesRecord != null)
        {
          obj = valuesRecord[propertyName];
          if (obj == DBNull.Value)
            obj = (object) null;
        }
        this._propertyValues[propertyName] = new ClonedPropertyValuesItem(propertyName, obj, propertyValuesItem.Type, propertyValuesItem.IsComplex);
      }
    }

    protected override IPropertyValuesItem GetItemImpl(string propertyName) => (IPropertyValuesItem) this._propertyValues[propertyName];

    public override ISet<string> PropertyNames => this._propertyNames;
  }
}
