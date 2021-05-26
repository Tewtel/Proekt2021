// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.IResultHandler2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  public interface IResultHandler2 : IResultHandler
  {
    /// <summary>Invoked when an error occurs.</summary>
    /// <param name="type"> The exception type. </param>
    /// <param name="message"> The error message. </param>
    /// <param name="stackTrace"> The stack trace. </param>
    /// <returns>true if the error was handled; otherwise, false.</returns>
    void SetError(string type, string message, string stackTrace);
  }
}
