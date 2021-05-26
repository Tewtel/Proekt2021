// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.IReportHandler
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  /// <summary>Used to handle reported design-time activity.</summary>
  public interface IReportHandler
  {
    /// <summary>Invoked when an error is reported.</summary>
    /// <param name="message">The message.</param>
    void OnError(string message);

    /// <summary>Invoked when a warning is reported.</summary>
    /// <param name="message">The message.</param>
    void OnWarning(string message);

    /// <summary>Invoked when information is reported.</summary>
    /// <param name="message">The message.</param>
    void OnInformation(string message);

    /// <summary>Invoked when verbose information is reported.</summary>
    /// <param name="message">The message.</param>
    void OnVerbose(string message);
  }
}
