// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Infrastructure.MigrationsLogger
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Migrations.Infrastructure
{
  /// <summary>
  /// Base class for loggers that can be used for the migrations process.
  /// </summary>
  public abstract class MigrationsLogger : MarshalByRefObject
  {
    /// <summary>Logs an informational message.</summary>
    /// <param name="message"> The message to be logged. </param>
    public abstract void Info(string message);

    /// <summary>Logs a warning that the user should be made aware of.</summary>
    /// <param name="message"> The message to be logged. </param>
    public abstract void Warning(string message);

    /// <summary>
    /// Logs some additional information that should only be presented to the user if they request verbose output.
    /// </summary>
    /// <param name="message"> The message to be logged. </param>
    public abstract void Verbose(string message);
  }
}
