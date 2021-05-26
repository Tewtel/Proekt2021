// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.DataSpace
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>DataSpace</summary>
  public enum DataSpace
  {
    /// <summary>OSpace indicates the item in the clr space</summary>
    OSpace,
    /// <summary>
    /// CSpace indicates the item in the CSpace - edm primitive types +
    /// types defined in csdl
    /// </summary>
    CSpace,
    /// <summary>SSpace indicates the item in the SSpace</summary>
    SSpace,
    /// <summary>Mapping between OSpace and CSpace</summary>
    OCSpace,
    /// <summary>Mapping between CSpace and SSpace</summary>
    CSSpace,
  }
}
