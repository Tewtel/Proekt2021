// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Represents the multiplicity information about the end of a relationship type
  /// </summary>
  public enum RelationshipMultiplicity
  {
    /// <summary>Lower Bound is Zero and Upper Bound is One</summary>
    ZeroOrOne,
    /// <summary>Both lower bound and upper bound is one</summary>
    One,
    /// <summary>Lower bound is zero and upper bound is null</summary>
    Many,
  }
}
