// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.DbConfigurationFinder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class DbConfigurationFinder
  {
    public virtual Type TryFindConfigurationType(
      Type contextType,
      IEnumerable<Type> typesToSearch = null)
    {
      return this.TryFindConfigurationType(contextType.Assembly(), contextType, typesToSearch);
    }

    public virtual Type TryFindConfigurationType(
      Assembly assemblyHint,
      Type contextTypeHint,
      IEnumerable<Type> typesToSearch = null)
    {
      if (contextTypeHint != (Type) null)
      {
        Type c = contextTypeHint.GetCustomAttributes<DbConfigurationTypeAttribute>(true).Select<DbConfigurationTypeAttribute, Type>((Func<DbConfigurationTypeAttribute, Type>) (a => a.ConfigurationType)).FirstOrDefault<Type>();
        if (c != (Type) null)
          return typeof (DbConfiguration).IsAssignableFrom(c) ? c : throw new InvalidOperationException(System.Data.Entity.Resources.Strings.CreateInstance_BadDbConfigurationType((object) c.ToString(), (object) typeof (DbConfiguration).ToString()));
      }
      List<Type> list = (typesToSearch ?? assemblyHint.GetAccessibleTypes()).Where<Type>((Func<Type, bool>) (t => t.IsSubclassOf(typeof (DbConfiguration)) && !t.IsAbstract() && !t.IsGenericType())).ToList<Type>();
      return list.Count <= 1 ? list.FirstOrDefault<Type>() : throw new InvalidOperationException(System.Data.Entity.Resources.Strings.MultipleConfigsInAssembly((object) list.First<Type>().Assembly(), (object) typeof (DbConfiguration).Name));
    }

    public virtual Type TryFindContextType(
      Assembly assemblyHint,
      Type contextTypeHint,
      IEnumerable<Type> typesToSearch = null)
    {
      if (contextTypeHint != (Type) null)
        return contextTypeHint;
      List<Type> list = (typesToSearch ?? assemblyHint.GetAccessibleTypes()).Where<Type>((Func<Type, bool>) (t => t.IsSubclassOf(typeof (DbContext)) && !t.IsAbstract() && !t.IsGenericType() && t.GetCustomAttributes<DbConfigurationTypeAttribute>(true).Any<DbConfigurationTypeAttribute>())).ToList<Type>();
      return list.Count != 1 ? (Type) null : list[0];
    }
  }
}
