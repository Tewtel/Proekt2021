// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.AssemblyExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Utilities
{
  internal static class AssemblyExtensions
  {
    public static string GetInformationalVersion(this Assembly assembly) => assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().Single<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public static IEnumerable<Type> GetAccessibleTypes(this Assembly assembly)
    {
      try
      {
        return assembly.DefinedTypes.Select<TypeInfo, Type>((Func<TypeInfo, Type>) (t => t.AsType()));
      }
      catch (ReflectionTypeLoadException ex)
      {
        return ((IEnumerable<Type>) ex.Types).Where<Type>((Func<Type, bool>) (t => t != (Type) null));
      }
    }
  }
}
