// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EdmSchemaErrorSeverity
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Defines the different severities of errors that can occur when validating an Entity Framework model.
  /// </summary>
  public enum EdmSchemaErrorSeverity
  {
    /// <summary>
    /// A warning that does not prevent the model from being used.
    /// </summary>
    Warning,
    /// <summary>An error that prevents the model from being used.</summary>
    Error,
  }
}
