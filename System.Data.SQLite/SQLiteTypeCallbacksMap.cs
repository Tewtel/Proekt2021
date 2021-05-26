﻿// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTypeCallbacksMap
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the mappings between database type names
  /// and their associated custom data type handling callbacks.
  /// </summary>
  internal sealed class SQLiteTypeCallbacksMap : Dictionary<string, SQLiteTypeCallbacks>
  {
    /// <summary>Constructs an (empty) instance of this class.</summary>
    public SQLiteTypeCallbacksMap()
      : base((IEqualityComparer<string>) new TypeNameStringComparer())
    {
    }
  }
}
