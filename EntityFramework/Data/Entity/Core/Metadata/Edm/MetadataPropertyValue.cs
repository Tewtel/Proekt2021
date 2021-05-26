// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MetadataPropertyValue
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class MetadataPropertyValue
  {
    private readonly PropertyInfo _propertyInfo;
    private readonly MetadataItem _item;

    internal MetadataPropertyValue(PropertyInfo propertyInfo, MetadataItem item)
    {
      this._propertyInfo = propertyInfo;
      this._item = item;
    }

    internal object GetValue() => this._propertyInfo.GetValue((object) this._item, new object[0]);
  }
}
