// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ParameterTypeSemantics
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// The enumeration defining the type semantics used to resolve function overloads.
  /// These flags are defined in the provider manifest per function definition.
  /// </summary>
  public enum ParameterTypeSemantics
  {
    /// <summary>
    /// Allow Implicit Conversion between given and formal argument types (default).
    /// </summary>
    AllowImplicitConversion,
    /// <summary>
    /// Allow Type Promotion between given and formal argument types.
    /// </summary>
    AllowImplicitPromotion,
    /// <summary>Use strict Equivalence only.</summary>
    ExactMatchOnly,
  }
}
