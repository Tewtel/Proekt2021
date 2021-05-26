// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EdmError
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// This class encapsulates the error information for a generic EDM error.
  /// </summary>
  [Serializable]
  public abstract class EdmError
  {
    private readonly string _message;

    internal EdmError(string message)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(message, nameof (message));
      this._message = message;
    }

    /// <summary>Gets the error message.</summary>
    /// <returns>The error message.</returns>
    public string Message => this._message;
  }
}
