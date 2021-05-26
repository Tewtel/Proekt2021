// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.ProviderRowFinder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Reflection;

namespace System.Data.Entity.Utilities
{
  internal class ProviderRowFinder
  {
    public virtual DataRow FindRow(
      Type hintType,
      Func<DataRow, bool> selector,
      IEnumerable<DataRow> dataRows)
    {
      AssemblyName assemblyName = hintType == (Type) null ? (AssemblyName) null : new AssemblyName(hintType.Assembly().FullName);
      foreach (DataRow dataRow in dataRows)
      {
        string typeName = (string) dataRow[3];
        AssemblyName rowProviderFactoryAssemblyName = (AssemblyName) null;
        Func<AssemblyName, Assembly> assemblyResolver = (Func<AssemblyName, Assembly>) (a =>
        {
          rowProviderFactoryAssemblyName = a;
          return (Assembly) null;
        });
        Type.GetType(typeName, assemblyResolver, (Func<Assembly, string, bool, Type>) ((_, __, ___) => (Type) null));
        if (rowProviderFactoryAssemblyName != null && (hintType == (Type) null || string.Equals(assemblyName.Name, rowProviderFactoryAssemblyName.Name, StringComparison.OrdinalIgnoreCase)))
        {
          try
          {
            if (selector(dataRow))
              return dataRow;
          }
          catch (Exception ex)
          {
          }
        }
      }
      return (DataRow) null;
    }
  }
}
