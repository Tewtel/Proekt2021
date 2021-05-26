// Decompiled with JetBrains decompiler
// Type: System.Net.Http.TypeExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Net.Http
{
  internal static class TypeExtensions
  {
    public static Type ExtractGenericInterface(this Type queryType, Type interfaceType)
    {
      Func<Type, bool> predicate = (Func<Type, bool>) (t => t.IsGenericType() && t.GetGenericTypeDefinition() == interfaceType);
      return !predicate(queryType) ? ((IEnumerable<Type>) queryType.GetInterfaces()).FirstOrDefault<Type>(predicate) : queryType;
    }

    public static bool IsGenericType(this Type type) => type.IsGenericType;

    public static bool IsInterface(this Type type) => type.IsInterface;

    public static bool IsValueType(this Type type) => type.IsValueType;
  }
}
