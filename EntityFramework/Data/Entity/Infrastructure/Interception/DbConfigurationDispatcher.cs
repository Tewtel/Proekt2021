// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.DbConfigurationDispatcher
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure.DependencyResolution;

namespace System.Data.Entity.Infrastructure.Interception
{
  internal class DbConfigurationDispatcher
  {
    private readonly System.Data.Entity.Infrastructure.Interception.InternalDispatcher<IDbConfigurationInterceptor> _internalDispatcher = new System.Data.Entity.Infrastructure.Interception.InternalDispatcher<IDbConfigurationInterceptor>();

    public System.Data.Entity.Infrastructure.Interception.InternalDispatcher<IDbConfigurationInterceptor> InternalDispatcher => this._internalDispatcher;

    public virtual void Loaded(
      DbConfigurationLoadedEventArgs loadedEventArgs,
      DbInterceptionContext interceptionContext)
    {
      DbConfigurationInterceptionContext clonedInterceptionContext = new DbConfigurationInterceptionContext(interceptionContext);
      this._internalDispatcher.Dispatch((Action<IDbConfigurationInterceptor>) (i => i.Loaded(loadedEventArgs, clonedInterceptionContext)));
    }
  }
}
