// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.MergeOption
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects
{
  /// <summary>
  /// The different ways that new objects loaded from the database can be merged with existing objects already in memory.
  /// </summary>
  public enum MergeOption
  {
    /// <summary>
    /// Will only append new (top level-unique) rows.  This is the default behavior.
    /// </summary>
    AppendOnly,
    /// <summary>Same behavior as LoadOption.OverwriteChanges.</summary>
    OverwriteChanges,
    /// <summary>Same behavior as LoadOption.PreserveChanges.</summary>
    PreserveChanges,
    /// <summary>Will not modify cache.</summary>
    NoTracking,
  }
}
