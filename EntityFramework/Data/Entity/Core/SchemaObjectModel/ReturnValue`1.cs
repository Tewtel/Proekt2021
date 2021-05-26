// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.ReturnValue`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class ReturnValue<T>
  {
    private bool _succeeded;
    private T _value;

    internal bool Succeeded => this._succeeded;

    internal T Value
    {
      get => this._value;
      set
      {
        this._value = value;
        this._succeeded = true;
      }
    }
  }
}
