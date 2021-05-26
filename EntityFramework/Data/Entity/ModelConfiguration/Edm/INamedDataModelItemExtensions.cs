// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.INamedDataModelItemExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class INamedDataModelItemExtensions
  {
    public static string UniquifyName(
      this IEnumerable<INamedDataModelItem> namedDataModelItems,
      string name)
    {
      return namedDataModelItems.Select<INamedDataModelItem, string>((Func<INamedDataModelItem, string>) (i => i.Name)).Uniquify(name);
    }
  }
}
