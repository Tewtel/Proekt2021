// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.IDbCommandTreeInterceptor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// An object that implements this interface can be registered with <see cref="T:System.Data.Entity.Infrastructure.Interception.DbInterception" /> to
  /// receive notifications when Entity Framework creates <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCommandTree" /> command trees.
  /// </summary>
  /// <remarks>
  /// Interceptors can also be registered in the config file of the application.
  /// See http://go.microsoft.com/fwlink/?LinkId=260883 for more information about Entity Framework configuration.
  /// </remarks>
  public interface IDbCommandTreeInterceptor : IDbInterceptor
  {
    /// <summary>
    /// This method is called after a new <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCommandTree" /> has been created.
    /// The tree that is used after interception can be changed by setting
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext.Result" /> while intercepting.
    /// </summary>
    /// <remarks>
    /// Command trees are created for both queries and insert/update/delete commands. However, query
    /// command trees are cached by model which means that command tree creation only happens the
    /// first time a query is executed and this notification will only happen at that time
    /// </remarks>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    void TreeCreated(
      DbCommandTreeInterceptionContext interceptionContext);
  }
}
