// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ContextConfig
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Data.Entity.Internal.ConfigFile;
using System.Linq;

namespace System.Data.Entity.Internal
{
  internal class ContextConfig
  {
    private readonly EntityFrameworkSection _entityFrameworkSettings;
    private readonly ConcurrentDictionary<Type, int?> _commandTimeouts = new ConcurrentDictionary<Type, int?>();

    public ContextConfig()
    {
    }

    public ContextConfig(EntityFrameworkSection entityFrameworkSettings) => this._entityFrameworkSettings = entityFrameworkSettings;

    public virtual int? TryGetCommandTimeout(Type contextType) => this._commandTimeouts.GetOrAdd(contextType, (Func<Type, int?>) (requiredContextType => this._entityFrameworkSettings.Contexts.OfType<ContextElement>().Where<ContextElement>((Func<ContextElement, bool>) (e => e.CommandTimeout.HasValue)).Select<ContextElement, int?>((Func<ContextElement, int?>) (e => ContextConfig.TryGetCommandTimeout(contextType, e.ContextTypeName, e.CommandTimeout.Value))).FirstOrDefault<int?>((Func<int?, bool>) (i => i.HasValue))));

    private static int? TryGetCommandTimeout(
      Type requiredContextType,
      string contextTypeName,
      int commandTimeout)
    {
      try
      {
        if (Type.GetType(contextTypeName, true) == requiredContextType)
          return new int?(commandTimeout);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.Database_InitializationException, ex);
      }
      return new int?();
    }
  }
}
