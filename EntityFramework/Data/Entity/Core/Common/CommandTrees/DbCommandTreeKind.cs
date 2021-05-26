// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbCommandTreeKind
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>
  /// Describes the different "kinds" (classes) of command trees.
  /// </summary>
  public enum DbCommandTreeKind
  {
    /// <summary>A query to retrieve data</summary>
    Query,
    /// <summary>Update existing data</summary>
    Update,
    /// <summary>Insert new data</summary>
    Insert,
    /// <summary>Deleted existing data</summary>
    Delete,
    /// <summary>Call a function</summary>
    Function,
  }
}
