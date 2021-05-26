// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.DefaultExecutionStrategyResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class DefaultExecutionStrategyResolver : IDbDependencyResolver
  {
    public object GetService(Type type, object key)
    {
      if (!(type == typeof (Func<IDbExecutionStrategy>)))
        return (object) null;
      System.Data.Entity.Utilities.Check.NotNull<object>(key, nameof (key));
      if (!(key is ExecutionStrategyKey))
        throw new ArgumentException(Strings.DbDependencyResolver_InvalidKey((object) typeof (ExecutionStrategyKey).Name, (object) "Func<IExecutionStrategy>"));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return (object) DefaultExecutionStrategyResolver.\u003C\u003Ec.\u003C\u003E9__0_0 ?? (object) (DefaultExecutionStrategyResolver.\u003C\u003Ec.\u003C\u003E9__0_0 = (Func<IDbExecutionStrategy>) (() => (IDbExecutionStrategy) new DefaultExecutionStrategy()));
    }

    public IEnumerable<object> GetServices(Type type, object key) => this.GetServiceAsServices(type, key);
  }
}
