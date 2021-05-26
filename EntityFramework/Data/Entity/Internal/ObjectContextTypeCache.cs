// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ObjectContextTypeCache
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Internal
{
  internal static class ObjectContextTypeCache
  {
    private static readonly ConcurrentDictionary<Type, Type> _typeCache = new ConcurrentDictionary<Type, Type>();

    public static Type GetObjectType(Type type) => ObjectContextTypeCache._typeCache.GetOrAdd(type, new Func<Type, Type>(ObjectContext.GetObjectType));
  }
}
