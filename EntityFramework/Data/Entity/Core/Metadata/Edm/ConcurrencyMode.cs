// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ConcurrencyMode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>The concurrency mode for properties.</summary>
  public enum ConcurrencyMode
  {
    /// <summary>
    /// Default concurrency mode: the property is never validated
    /// at write time
    /// </summary>
    None,
    /// <summary>
    /// Fixed concurrency mode: the property is always validated at
    /// write time
    /// </summary>
    Fixed,
  }
}
