// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IObjectContextAdapter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Interface implemented by objects that can provide an <see cref="P:System.Data.Entity.Infrastructure.IObjectContextAdapter.ObjectContext" /> instance.
  /// The <see cref="T:System.Data.Entity.DbContext" /> class implements this interface to provide access to the underlying
  /// ObjectContext.
  /// </summary>
  public interface IObjectContextAdapter
  {
    /// <summary>Gets the object context.</summary>
    /// <value> The object context. </value>
    ObjectContext ObjectContext { get; }
  }
}
