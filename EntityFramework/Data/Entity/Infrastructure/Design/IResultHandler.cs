// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.IResultHandler
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  /// <summary>
  /// A contract handlers can use to accept a single result.
  /// </summary>
  /// <seealso cref="T:System.Data.Entity.Infrastructure.Design.HandlerBase" />
  public interface IResultHandler
  {
    /// <summary>Sets the result.</summary>
    /// <param name="value">The result.</param>
    void SetResult(object value);
  }
}
