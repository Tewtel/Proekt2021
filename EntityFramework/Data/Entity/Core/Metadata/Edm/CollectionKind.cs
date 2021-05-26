// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.CollectionKind
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Kind of collection (applied to Properties)</summary>
  public enum CollectionKind
  {
    /// <summary>Property is not a Collection</summary>
    None,
    /// <summary>
    /// Collection has Bag semantics( unordered and duplicates ok)
    /// </summary>
    Bag,
    /// <summary>
    /// Collection has List semantics
    /// (Order is deterministic and duplicates ok)
    /// </summary>
    List,
  }
}
