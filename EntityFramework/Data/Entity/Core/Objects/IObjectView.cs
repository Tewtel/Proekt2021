// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.IObjectView
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;

namespace System.Data.Entity.Core.Objects
{
  internal interface IObjectView
  {
    void EntityPropertyChanged(object sender, PropertyChangedEventArgs e);

    void CollectionChanged(object sender, CollectionChangeEventArgs e);
  }
}
