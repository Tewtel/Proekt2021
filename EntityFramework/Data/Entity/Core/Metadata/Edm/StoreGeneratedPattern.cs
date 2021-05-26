// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.StoreGeneratedPattern
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>The pattern for Server Generated Properties.</summary>
  public enum StoreGeneratedPattern
  {
    /// <summary>Not a Server Generated Property. This is the default.</summary>
    None,
    /// <summary>
    /// A value is generated on INSERT, and remains unchanged on update.
    /// </summary>
    Identity,
    /// <summary>A value is generated on both INSERT and UPDATE.</summary>
    Computed,
  }
}
