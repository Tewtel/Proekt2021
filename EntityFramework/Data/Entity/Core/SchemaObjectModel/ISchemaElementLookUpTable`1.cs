// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.ISchemaElementLookUpTable`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal interface ISchemaElementLookUpTable<T> where T : SchemaElement
  {
    int Count { get; }

    bool ContainsKey(string key);

    T this[string key] { get; }

    IEnumerator<T> GetEnumerator();

    T LookUpEquivalentKey(string key);
  }
}
